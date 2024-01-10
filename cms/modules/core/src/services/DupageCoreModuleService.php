<?php

namespace modules\DupageCoreModule\services;

use Craft;
use DOMDocument;
use DateTime;
use PDO;
use craft\base\Component;
use craft\base\Element;
use craft\elements\Asset;
use craft\elements\Category;
use craft\elements\Entry;
use craft\elements\db\EntryQuery;
use craft\helpers\UrlHelper;
use ether\simplemap\services\GeoService;
use modules\DupageCoreModule\DupageCoreModule;
use modules\physiciansmodule\PhysiciansModule;
use modules\physiciansmodule\controllers\tntsearch\Connectors\TNTMySqlConnector;
use modules\physiciansmodule\controllers\tntsearch\TNTSearchDMG;
use modules\physiciansmodule\forms\PhysicianSearchForm;
use punchkick\eventregistration\Elements\Event;
use samdark\log\PsrMessage;
use yii\base\Exception;
use yii\db\ArrayExpression;
use yii\db\Expression;
use yii\db\Query;
use yii\httpclient\Client as YiiHttpClient;
use yii\httpclient\Exception as HttpClientException;
use yii\httpclient\Response;

class DupageCoreModuleService extends Component
{
    // Public Methods
    // =========================================================================

    /**
     * This method validates provided raCAPTCHA with Google
     *
     * @return bool Whether provided reCAPTCHA token is valid
     */
    public function verifyRecaptchaToken($token = null): bool
    {
        $client = new YiiHttpClient();
        $response = $client->createRequest()
            ->setMethod('POST')
            ->setUrl('https://www.google.com/recaptcha/api/siteverify')
            ->setData([
                'secret' => Craft::$app->config->general->reCAPTCHASiteSecret,
                'response' => $token
            ])
            ->send();

        $parsedResponse = \json_decode($response->getContent(), true);

        if (!$parsedResponse) {
            Craft::error(new PsrMessage('Unable to validate reCAPTCHA token. /siteverify returned no response.', [
                'status' => $response->statusCode
            ]));
            return false;
        }

        if ($parsedResponse['success'] == false) {
            Craft::error(new PsrMessage('Invalid ReCAPTCHA token.', [
                'token' => $token,
                'error-codes' => $parsedResponse['error-codes']
            ]));
            return false;
        }

        return true;
    }

    /**
     * Returns an instance of TNTSearch
     */
    public function getTntSearch()
    {
        return self::getTntSearchStatic();
    }

    /**
     * Returns a TNTSearch index
     */
    public static function getTntIndex()
    {
        return self::getTntIndexStatic();
    }

    /**
     * Returns an instance of TNTSearch
     */
    public static function getTntSearchStatic()
    {
        $tnt = new TNTSearchDMG;
        $connector = self::getTntConnector();
        $tnt->setDatabaseHandle($connector->connect(self::getTntConfig()));
        $tnt->loadConfig(self::getTntConfig());

        return $tnt;
    }

    /**
     * Returns a TNTSearch index
     */
    public static function getTntIndexStatic()
    {
        $tnt = self::getTntSearchStatic();
        $connector = self::getTntConnector();
        $tnt->selectIndex("dmgtntindex", $connector->connect(self::getTntConfig()));
        return $tnt;
    }

    /**
     * Given a Craft entry element, updates the TNT search index for the given entry.
     *
     * If a given entry was never indexed, this update will still process it and save it.
     */
    public function updateIndexForEntry($entry)
    {
        Craft::beginProfile('serialized_entry_field', __METHOD__);
        $serializedEntryFields = self::serializeFieldValues($entry);
        Craft::endProfile('serialized_entry_field', __METHOD__);

        Craft::beginProfile('update_tnt_index', __METHOD__);

        $transaction = Craft::$app->db->beginTransaction();
        try {
            // get the index
            $index = $this->getTntIndex()->getIndex();

            // update the index
            $index->update($serializedEntryFields['id'], $serializedEntryFields);
            $transaction->commit();
        } catch (\Exception $e) {
            $transaction->rollback();
        }

        Craft::endProfile('update_tnt_index', __METHOD__);
    }

    /**
     * Recursive function that queries all of the fields of the given entry, parses out and normalizes useful values.
     *
     * The result is an associative array of all relevant details about the entry.
     *
     * Null/true/false/empty values are discarded.
     *
     * For this particular index, entry's title's matches should carry more weight that other properties.
     * As such, the entry's title is repeated x1000 in the final collection of attributes.
     * This is a simply way to trick the index to weight the title more.
     * This works because TNTSearch index search is a TF-IDF-like retrieval functions,
     * i.e. it uses statistical models to determine how important a word is to a document in a collection.
     *
     */
    public static function serializeFieldValues($entry, $loop = 0)
    {
        $serializedData = [];

        // since this function is recursive, we can add a prefix to all the keys to prevent keypair overwriting by other recursive stack layers
        $keyPrefix = $entry->id . "_";
        foreach ($entry->getFieldValues() as $key => $val) {
            if (is_object($val)) {
                $type = get_class($val);
                if (is_a($val, 'craft\elements\db\ElementQuery')) {
                    // if this is an element query, then grab all elements and return element title
                    // and save the results
                    $num = 0;
                    foreach ($val->all() as $element) {
                        $serializedData[$keyPrefix . $key. "_" . $num] = $element->title;
                        $num++;
                    }
                } elseif (is_a($val, 'ether\simplemap\models\Map')) {
                    // if this is an instance of a map, extract the address
                    // and make it safe for web display
                    $serializedData[$keyPrefix . $key] = htmlspecialchars(strip_tags($val->address()->jsonSerialize()));
                } elseif (is_a($val, 'craft\redactor\FieldData')) {
                    // if this is an instance of a redactor field (a wysiwyg), get the raw value
                    // and make it safe for web display
                    $serializedData[$keyPrefix . $key] = htmlspecialchars(strip_tags($val->getRawContent()));
                } elseif (is_a($val, 'craft\fields\data\SingleOptionFieldData')) {
                    // if this is an instance of a options field, just serialize the value
                    $serializedData[$keyPrefix . $key] = $val->serialize();
                }
            } else {
                // if not an object, just save the value at $key
                $serializedData[$keyPrefix . $key] = \is_array($val) ? \json_encode($val) : $val;
            }
        }

        $serializedData['id'] = $entry->id;

        // trick the search to weigh the title more heavily
        $serializedData[$keyPrefix . 'title'] = str_repeat($entry->title . " ", 1000);

        // drop null/true/false/empty values
        $serializedData = \array_filter(
            $serializedData,
            function ($value) {
                return !is_bool($value) and $value !== null;
            }
        );

        return $serializedData;
    }

    /**
     * Convenience function to generate a secure image url
     *
     * @param string $url
     */
    public function generateImageSignature(string $url, string $extension)
    {
        $key = pack("H*", Craft::$app->config->general->imgproxy_key);
        if (empty($key)) {
            return null;
        }

        $salt = pack("H*", Craft::$app->config->general->imgproxy_salt);
        if (empty($salt)) {
            return null;
        }

        $path = '/' . $url . '.' . $extension;
        $data = $salt . $path;

        $sha256 = hash_hmac('sha256', $data, $key, true);
        $sha256Encoded = base64_encode($sha256);
        $signature = str_replace(["+", "/", "="], ["-", "_", ""], $sha256Encoded);

        return UrlHelper::baseSiteUrl() . \str_replace('//', '/', "assets/{$signature}/{$path}");
    }

    public function getTransformedAssetFilePath($asset)
    {
        if (\get_class($asset->volume) === 'craft\\volumes\\Local') {
            return \base64_encode('local:///' . \str_replace('@assetBaseUrl/', '', $asset->volume->url) . '/' .  $asset->path);
        } else {
            $url = "https://" . \getenv(\str_replace('$', '', $asset->volume->accountName)) . '.blob.core.windows.net/' . \getenv(\str_replace('$', '', $asset->volume->containerName)) . '/' . \str_replace('@assetBaseUrl', '', $asset->volume->url) . $asset->volume->subfolder . '/' . $asset->path;
            return \base64_encode($url);
        }
    }

    public function htmlSpecialCharsDecode(array $values)
    {
        return \array_map(
            function ($value) {
                return htmlspecialchars_decode($value);
            },
            $values
        );
    }

    /**
     * Retrieves the Press Ganey ratings and comments for a given provider
     *
     * @param Entry $entry
     * @return float
     */
    public function getProviderRatingAndComments(Entry $entry)
    {
        if ($entry->section->handle !== 'physicians') {
            throw new Exception('An entry with the wrong section was provided');
        }

        $duration = 15 * 24 * 3600; // 15 days
        $physicianNPINumber = $entry->nationalProviderIdentifier;

        if (!$physicianNPINumber) {
            return null;
        }

        $ratingAndComments = Craft::$app->cache->get("physician_{$physicianNPINumber}_ratings_and_comments");

        return $ratingAndComments;
    }

    /**
     * Performs a site-wide search for a given query.
     *
     * The search index returns only document IDs. This fuction queries the DB for appropriate entries.
     * It parses the results into a collection of objects including relevant entry information.
     *
     * Image URLs are generated using the generateImageSignature function that is used to generate all other images throughout the site.
     */
    public function siteWideSearch($query, $sections, $page, $perPage)
    {
        $defaultSections = ['physicians', 'blog', 'services', 'locations', 'conditions'];
        $appliedSections = [];

        if ($sections) {
            if (is_array($sections)) {
                $appliedSections = $sections;
            } else {
                \array_push($appliedSections, $sections);
            }
        } else {
            $appliedSections = $defaultSections;
        }

        $tnt = DupageCoreModule::getInstance()->dupageCoreModuleService->getTntIndex();

        // get result IDs
        $searchResults = $tnt->search($query)['ids'];

        $searchIds = [];

        foreach ($appliedSections as $section) {
            $sectionResult = null;
            // apply filters
            if (\in_array($section, ['physicians', 'blog', 'services', 'locations', 'conditions'])) {
                $sectionResult = Entry::find()->section($section)->id($searchResults)->fixedOrder(true)->ids();
            } elseif ($section === 'events') {
                $sectionResult = Event::buildQuery(['rangeStart' => 'now', 'rangeEnd' => '+1 year'])->id($searchResults)->ids();
            } elseif ($section === 'all') {
                $sectionResult = Entry::find()->section($defaultSections)->id($searchResults)->fixedOrder(true)->ids();
            }

            if ($sectionResult) {
                $searchIds = \array_merge(
                    $searchIds,
                    $sectionResult
                );
            }
        };

        // run the result IDs through a Craft Entries query to weed out any "invalid" entry IDs
        // e.g. entries that are now disabled, or entries that otherwise shouldn't be shown to the user at this time
        $entries = Entry::find()->with(['physicianHeadshot', 'heroImage', 'serviceHeroImage'])->id($searchIds)->fixedOrder(true)->all();

        // also find matching events, if any
        $events = Event::buildQuery(['rangeStart' => 'now', 'rangeEnd' => '+1 year'])->id($searchIds)->all();

        $elements = \array_merge(
            $events,
            $entries,
        );

        // above merge would have destroyed the original order of the results as determined by the index search
        // so we need to restore it
        \usort(
            $elements,
            function ($a, $b) use ($searchResults) {
                return \array_search($a->id, $searchResults) > \array_search($b->id, $searchResults);
            }
        );

        $res = [
                'physicians' => [],
                'services' => [],
                'conditions' => [],
                'procedures' => [],
                'locations' => [],
                'blog' => [],
                'event' => []
        ];

        foreach ($elements as $element) {
            $entrySectionHandle = $element->section->handle ?? "event";

            // base object for all result types
            $item = [
                'id' => $element->id,
                'type' => $entrySectionHandle,
                'url' => $element->url,
                'title' => $element->title
            ];

            $item = $this->applySectionSpecificAttributes($entrySectionHandle, $item, $element);

            if ($item) {
                \array_push(
                    $res[$entrySectionHandle],
                    $item
                );
            }
        }

        // flatten the array by removing the keys and merging the values
        // while keeping this specific grouping order
        return \array_merge($res['services'], $res['physicians'], $res['conditions'], $res['procedures'], $res['locations'], $res['blog'], $res['event']);
    }

    /**
     * Returns a list of auto-suggestions for given query for entries from specifies sections,
     * ordered by how closely they match the given query.
     *
     * @param string $query search query
     * @param string[] $sections results will be pulled only from these Craft sections
     * @param int $limit number of results returned
     */
    public function getAutoSuggestions($query, $sections, $limit = 5)
    {
        // hard requirement--autosuggestions require the query to be at least 3 characters
        if (\strlen($query) < 3) {
            return [];
        }

        // Extract the stem of the word that must be exact
        // This stem must be at the beginning of result titles, (or beginning of any word for multi-word titles), per requirements
        // The value of 5 was chosen as the best-suited value through some experimentation
        $stem = \strtolower(\substr($query, 0, 5));

        // find all entries from given sections where titles contain the stem
        $elementIds = Entry::find()
            ->section($sections)
            ->search("title:{$stem}* OR alternativeSearchName:{$stem}* OR alternativeSearchServiceName:{$stem}*");

        // when looking through locations, exclude suites
        if (\in_array("locations", $sections)) {
            $elementIds = $elementIds->type('location');
        }

        $elementIds = $elementIds->ids();

        // find entry titles
        $titles = (new Query())
            ->select(['title'])
            ->from('content')
            ->where(['in', 'elementId', $elementIds])
            ->all();

        // return [] if no results are found
        if (\count($titles) == 0) {
            return $titles;
        }

        // extract the 'title' attribute for more concise code later
        $titles = \array_map(
            fn ($title) => $title['title'],
            $titles
        );

        $titles = $this->orderStringsByClosestMatch($titles, $query, $stem);

        // return only yop 5 results
        $titles = \array_slice($titles, 0, $limit);

        // sort A-Z
        \sort($titles);

        return $titles;
    }
    /**
     * Returns a list of auto-suggestions for given query for entries from specifies sections,
     * ordered by how closely they match the given query.
     *
     * @param string[] $strings collection of strings to order
     * @param string $query search query
     * @param string $stem a word stem extracted from the search query
     */
    private function orderStringsByClosestMatch($strings = [], $query = "", $stem = "")
    {
        \usort($strings, function ($a, $b) use ($query, $stem) {
            // normalize casing
            $a = \strtolower($a);
            $b = \strtolower($b);

            // Single-word strings will begin with a stem. E.g. "Cardiology" will be returned for stem "ca" (query "cardio").
            // At least one word in multi-word strings will begin with a stem. E.g. "Michael P. Murphy, DO" will be returned for stem "Mu" (query "murphy").

            // Before we measure levenshtein distances, we want to create better working strings from the returned strings.

            // First problem:
            // If we query for "murphy", our stem is "mu".
            // The results set includes both "Muscle Biopsy" but also "Brian Murphy, MD"
            // Simple levenshtein distance comparison against the query will put "Muscle Biopsy" first (distance of 10 vs Brian's 11)!

            // We find the position of the stem in the title and cut off everything before it. This fixes the first issue
            $modStrA = \substr($a, \stripos($a, $stem));
            $modStrB = \substr($b, \stripos($b, $stem));

            // Second problem:
            // For the same query, "MUGA Testing" is returned before "Kimberly Murphy, MD, FACEP".
            // After above modification, we are left with "MUGA Testing" and "Murphy, MD, FACEP"
            // Simply due to the fact that "MUGA Testing" is shorter, it will have a smaller levenshtein distances
            // from the query compared to the other string.

            // We fix this by padding all modified strings (already beggining with the stem) to be the exact same length.
            // A logical length would be the query (this is all of the search context that we got from the user)

            $queryLength = \strlen($query);
            // pad smaller strings
            $modStrA = \str_pad($modStrA, $queryLength, "#");
            $modStrB = \str_pad($modStrB, $queryLength, "#");
            // crop larger strings
            $modStrA = \substr($modStrA, 0, $queryLength);
            $modStrB = \substr($modStrB, 0, $queryLength);

            // finally, calculate the distances
            $levA = \levenshtein($query, $modStrA);
            $levB = \levenshtein($query, $modStrB);

            return $levA === $levB ? 0 : ($levA > $levB ? 1 : -1);
        });

        return $strings;
    }

    private function applySectionSpecificAttributes($entrySectionHandle, &$item, $element)
    {
        switch ($entrySectionHandle) {
            case 'physicians':
                // specifically for physicians, add the rating and the image
                $rating = DupageCoreModule::getInstance()->dupageCoreModuleService->getProviderRatingAndComments($element);
                if (!$rating) {
                    $item['rating'] = [
                        'overallRating' => $rating && $rating['overallRating'] ? $rating['overallRating']['value'] : null,
                        'commentsCount' => $rating && $rating['overallRating'] ? count($rating['comments']) : null
                    ];
                }
                $item['physicianSpeciality'] = $element->physicianSpeciality;
                $item['img'] = count($element->physicianHeadshot) > 0 ? $element->physicianHeadshot[0] : null;
                return $item;

            case 'blog':
                // specifically for blog items, add the summary and the image
                $item['summary'] = \htmlspecialchars(\strip_tags(\mb_strimwidth($element->contentSummary, 0, 256, "...")));
                $item['img'] = count($element->heroImage) > 0 ? $element->heroImage[0] : null;
                return $item;

            case 'services':
                // specifically for services, add the summary and the image
                $item['summary'] = \htmlspecialchars(\strip_tags(\mb_strimwidth($element->serviceDescription, 0, 256, "...")));
                $item['img'] = count($element->serviceHeroImage) > 0 ? $element->serviceHeroImage[0] : null;
                return $item;

            case 'event':
                // specifically for events, add the url, title and summary
                $item['summary'] = \htmlspecialchars(strip_tags($this->entryWithOptimizedImages($element, 'eventDescription')));
                return $item;

            case 'conditions':
                // for conditions, just return the basic object
                return $item;

            case 'procedures':
                // for procedures, just return the basic object
                return $item;

            case 'locations':
                // do not return suites, only physical locations
                $element = $element->parent ?? $element;
                // if suite, get parent's information
                $item = [
                    'id' => $element->id,
                    'type' => $entrySectionHandle,
                    'url' => $element->url,
                    'title' => $element->title
                ];
                return $item;

            default:
                // all other sections are excluded
                return null;
                break;
        }
    }

    /**
     * Wrapper function for retrieving the Press Ganey ratings and comments for a given provider
     *
     */
    public function getPressGaneyRatingsAndComments(string $physicianNPINumber, int $attempt = 0)
    {
        // attempt to get physician ratings and comments
        $ratingAndComments = $this->getPressGaneyRatingsAndCommentsHelper($physicianNPINumber);
        if ($ratingAndComments && $ratingAndComments['status']['code'] == 200) {
            Craft::info(new PsrMessage('Cached physician Press Ganey ratings and comments for NPI', [
                'npi' => $physicianNPINumber
            ]));

            return $ratingAndComments['data']['entities'][0];
        } elseif ($ratingAndComments && $ratingAndComments['status']['code'] == 401) {
            // Access token may be invalid. Delete current cached copy and try again.
            Craft::$app->cache->delete('press_ganey_access_token');

            // If the recursive fn has been called twice or more, give up, something else is wrong
            if ($attempt < 2) {
                Craft::warning(new PsrMessage('Unable to cache physician Press Ganey ratings and comments for NPI. Trying again.', [
                    'npi' => $physicianNPINumber,
                    'response' => $ratingAndComments
                ]));

                // Allow for an additional retry of the token expired mid session
                return $this->getPRessGaneyRatingsAndComments($physicianNPINumber, (++$attempt));
            }
        }

        Craft::error(new PsrMessage('Unable to cache physician Press Ganey ratings and comments for NPI. Giving up!', [
            'npi' => $physicianNPINumber,
            'response' => $ratingAndComments
        ]));

        return false;
    }

    /**
     * Generates clean HTML markup with embeded asset images and videos using getOptimizedImage
     *
     * @param Element $entry
     * @param string $field
     * @return string
     */
    public function entryWithOptimizedImages(Element $entry, $field = 'blogContent')
    {
        $dom = new DOMDocument;
        libxml_use_internal_errors(true);
        $content = (string)$entry->$field->getRawContent();

        $dom->loadHtml('<?xml encoding=\"utf-8\" ?>' . $content, LIBXML_HTML_NOIMPLIED | LIBXML_HTML_NODEFDTD);
        libxml_clear_errors();

        $images = $dom->getElementsByTagName('img');
        for ($i = 0; $i < $images->length; $i++) {
            $img = $images->item($i);
            $sibling = $img->nextSibling;

            $id = \str_replace(['{', '}', 'asset', ':', 'url'], '', $img->getAttribute('src'));
            $asset = Asset::find()->id($id)->one();

            if ($asset !== null) {
                $picture = new DOMDocument;
                libxml_use_internal_errors(true);
                $metadata = [];
                if ($sibling !== null && $sibling->tagName == "figcaption") {
                    $data = \json_decode($sibling->nodeValue, true);
                    if (json_last_error() == JSON_ERROR_NONE) {
                        $metadata = $data;
                    }
                }
                $picture->loadHtml($this->getOptimizedImage($asset, 'webp', false, [
                    ['settings' => ['gravity:sm', 'resize:fill:1200::1:1'], 'css' => '(min-width: 1200px)'],
                    ['settings' => ['gravity:sm', 'resize:fill:992::1:1'], 'css' => '(min-width: 992px)'],
                    ['settings' => ['gravity:sm', 'resize:fill:768::1:1'], 'css' => '(min-width: 768px)'],
                    ['settings' => ['gravity:sm', 'resize:fill:576::1:1'], 'css' => '(min-width: 576px)']
                ], null, $metadata), LIBXML_HTML_NOIMPLIED | LIBXML_HTML_NODEFDTD);
                libxml_clear_errors();
                $node = $dom->importNode($picture->documentElement, true);

                $img->parentNode->parentNode->replaceChild($node, $img->parentNode);
            } else {
                // Remove unprocessable images from the output
                $img->parentNode->parentNode->removeChild($img->parentNode);
            }
        }

        $links = $dom->getElementsByTagName('a');
        for ($i = 0; $i < $links->length; $i++) {
            $a = $links->item($i);
            $href = $a->getAttribute('href');

            if (\preg_match('/{entry:\d+:url}/', $href)) {
                $id = \str_replace(['{', '}', 'entry', ':', 'url'], '', $href);
                $entry = Entry::find()->id($id)->one();

                if ($entry === null) {
                    $a->setAttribute('disabled', 'disabled');
                    $a->setAttribute('class', 'disabled');
                    $a->removeAttribute('href');
                } else {
                    $a->setAttribute('href', $entry->getUrl());
                }
            }

            if (\preg_match('/{category:\d+:url}/', $href)) {
                $id = \str_replace(['{', '}', 'category', ':', 'url'], '', $href);
                $entry = Category::find()->id($id)->one();

                if ($entry === null) {
                    $a->setAttribute('disabled', 'disabled');
                    $a->setAttribute('class', 'disabled');
                    $a->removeAttribute('href');
                } else {
                    $a->setAttribute('href', $entry->getUrl());
                }
            }
        }

        return $dom->saveHtml();
    }

    private function getAssetUrl($asset)
    {
        return DupageCoreModule::getInstance()->dupageCoreModuleService->generateImageSignature(
            implode('/', ['gravity:sm', 'resize:fill:500:500:1:1'])
                . '/'
                . DupageCoreModule::getInstance()->dupageCoreModuleService->getTransformedAssetFilePath($asset),
            'jpg'
        );
    }

    /**
     * Returns a Press Ganey Access token from either cache or HTTP call
     * @return string|null
     */
    private function getPressGaneyAccessToken()
    {
        $cacheToken = Craft::$app->cache->get('press_ganey_access_token');
        if (!isset($cacheToken) || $cacheToken === false) {
            $client = Craft::$app->httpclient->getClient();

            $appId = Craft::$app->config->general->pressGaneyAppId;
            $appSecret = Craft::$app->config->general->pressGaneyAppSecret;
            $url = "https://api.binaryfountain.com/api/service/v1/token/create";

            $request = $client->createRequest()
                ->setMethod('POST')
                ->setFormat($client::FORMAT_RAW_URLENCODED)
                ->setHeaders([
                    'Content-Type' => 'application/x-www-form-urlencoded',
                    'Accept' => 'application/json'
                ])
                ->setContent("appId={$appId}&appSecret={$appSecret}")
                ->setUrl($url);

            $response = $request->send();

            if ($response->getHeaders()->get('http-code') != 200) {
                // If we get a non 200 back, log the url and action for analysis later
                Craft::error(new PsrMessage('Failed to generate Press Ganey API token', [
                    'url' => $url,
                    'headers' => $response->getHeaders()->toArray(),
                    'response' => trim(preg_replace('/[\r\n]+/', '', $response->getContent())),
                ]), get_class($this) . '::' . __METHOD__);

                return null;
            }

            $body = $response->getData();
            $expirationDate = DateTime::createFromFormat("Y-m-d H:i:s", $body['expiresIn']);
            $currentDate = new DateTime();

            $duration = $expirationDate->getTimestamp() - $currentDate->getTimestamp();

            $duration -= 60;
            $cacheToken = $body['accessToken'];
            Craft::$app->cache->set('press_ganey_access_token', $cacheToken, $duration);
        }

        return $cacheToken;
    }

    /**
     * Makes a POST request to binaryfountain to retrieve physician ratings and comments
     *
     * @param string $physicianNPINumber
     * @return array|null
     */
    private function getPressGaneyRatingsAndCommentsHelper(string $physicianNPINumber)
    {
        if ($physicianNPINumber <= 1) {
            Craft::error(new PsrMessage('Invalid NPI provided to Press Ganey - Skipping', [
                'npi' => $physicianNPINumber
            ]));
            return null;
        }

        $client = Craft::$app->httpclient->getClient();
        $url = "https://api.binaryfountain.com/api/service/bsr/comments";

        $accessToken = $this->getPressGaneyAccessToken();
        if ($accessToken == null) {
            return null;
        }

        $request = $client->createRequest()
            ->setMethod('POST')
            ->setFormat($client::FORMAT_RAW_URLENCODED)
            ->setHeaders([
                'Content-Type' => 'application/x-www-form-urlencoded',
                'Accept' => 'application/json',
                'accessToken' => $accessToken
            ])
            ->setContent("personId={$physicianNPINumber}")
            ->setUrl($url);

        $response = $request->send();

        if ($response->getHeaders()->get('http-code') != 200) {
            // If we get a non 200 back, log the url and action for analysis later
            Craft::error(new PsrMessage('Failed to get comments and ratings form PRess Ganey API', [
                'url' => $url,
                'npi' => $physicianNPINumber,
                'headers' => $response->getHeaders()->toArray(),
                'response' => trim(preg_replace('/[\r\n]+/', '', $response->getContent()))
            ]), get_class($this) . '::' . __METHOD__);

            return null;
        }

        return $response->getData();
    }

    /**
     * Generate the configuration required by TNTSearch
     * TNTSearch is setup to connect to the existing Craft MySQL database
     */
    private static function getTntConfig()
    {
        return [
            'driver' => getenv('DB_DRIVER'),
            'host' => getenv('DB_SERVER'),
            'database' => getenv('DB_DATABASE'),
            'username' => getenv('DB_USER'),
            'password' => getenv('DB_PASSWORD'),
            'storage' => ''
        ];
    }

    /**
     * Sets up the MySQL database connector for TNTSearch.
     * This project's MySql connection requires certain SSL options so that TNTSearch can connect to our DB to query data.
     */
    private static function getTntConnector()
    {
        return new TNTMySqlConnector();
    }

    public function getOptimizedImage(
        Asset $asset,
        string $extension = 'webp',
        bool $asArray = false,
        array $settings = [
            ['settings' => ['gravity:sm', 'resize:fill:1200:675:1:1'], 'css' => '(min-width: 1200px)'],
            ['settings' => ['gravity:sm', 'resize:fill:992:558:1:1'], 'css' => '(min-width: 992px)'],
            ['settings' => ['gravity:sm', 'resize:fill:768:432:1:1'], 'css' => '(min-width: 768px)'],
            ['settings' => ['gravity:sm', 'resize:fill:576:324:1:1'], 'css' => '(min-width: 576px)']
        ],
        string $class = null,
        array $metadata = []
    ) {
        $filePath = $this->getTransformedAssetFilePath($asset);

        $dom = new DOMDocument;
        libxml_use_internal_errors(true);
        $dom->loadHtml('<picture></picture>', LIBXML_HTML_NOIMPLIED | LIBXML_HTML_NODEFDTD);
        libxml_clear_errors();

        $urls = [];

        $picture = $dom->getElementsByTagName('picture')[0];

        foreach ($settings as $setting) {
            $srcSet = null;
            if (isset($setting['css'])) {
                $srcSet = $setting['css'];
                unset($setting['css']);
            }

            $generatedUrls = [
                $this->generateImageSignature(implode('/', $setting['settings']) . '/' . $filePath, $extension),
                $this->generateImageSignature(implode('/', $setting['settings']) . '/' . $filePath, 'jpg')
            ];

            foreach ($generatedUrls as $url) {
                $urls[] = $url;
                if ($srcSet !== null) {
                    $source = $dom->createElement('source');
                    $source->setAttribute('media', $srcSet);
                    $source->setAttribute('srcset', $url);
                    if (\substr($url, -4) === '.jpg') {
                        $source->setAttribute('type', 'image/jpeg');
                    } elseif (\substr($url, -5) === '.webp') {
                        $source->setAttribute('type', 'image/webp');
                    }

                    $picture->appendChild($source);
                }
            }
        }

        $img = $dom->createElement('img');
        $img->setAttribute('src', \str_replace('.webp', '.jpg', $url));
        $img->setAttribute('alt', $asset->imageAltText);
        if (isset($metadata['css'])) {
            $img->setAttribute('style', $metadata['css']);
        }

        if (isset($metadata['class']) && $class === null) {
            $class = $metadata['class'];
        }

        if ($class !== null) {
            $img->setAttribute('class', $class);
        }

        $picture->appendChild($img);

        if ($asArray === true) {
            return $urls;
        }

        return $dom->saveHtml();
    }

    /* -----------------------------------
    * CONVERT STATE NAMES!
    * Goes both ways. e.g.
    * $name = 'Orgegon' -> returns "OR"
    * $name = 'OR' -> returns "Oregon"
    * ----------------------------------- */
    public function convertState($name)
    {
        $states = array(
            array('name' => 'Alabama', 'abbr' => 'AL'),
            array('name' => 'Alaska', 'abbr' => 'AK'),
            array('name' => 'Arizona', 'abbr' => 'AZ'),
            array('name' => 'Arkansas', 'abbr' => 'AR'),
            array('name' => 'California', 'abbr' => 'CA'),
            array('name' => 'Colorado', 'abbr' => 'CO'),
            array('name' => 'Connecticut', 'abbr' => 'CT'),
            array('name' => 'Delaware', 'abbr' => 'DE'),
            array('name' => 'Florida', 'abbr' => 'FL'),
            array('name' => 'Georgia', 'abbr' => 'GA'),
            array('name' => 'Hawaii', 'abbr' => 'HI'),
            array('name' => 'Idaho', 'abbr' => 'ID'),
            array('name' => 'Illinois', 'abbr' => 'IL'),
            array('name' => 'Indiana', 'abbr' => 'IN'),
            array('name' => 'Iowa', 'abbr' => 'IA'),
            array('name' => 'Kansas', 'abbr' => 'KS'),
            array('name' => 'Kentucky', 'abbr' => 'KY'),
            array('name' => 'Louisiana', 'abbr' => 'LA'),
            array('name' => 'Maine', 'abbr' => 'ME'),
            array('name' => 'Maryland', 'abbr' => 'MD'),
            array('name' => 'Massachusetts', 'abbr' => 'MA'),
            array('name' => 'Michigan', 'abbr' => 'MI'),
            array('name' => 'Minnesota', 'abbr' => 'MN'),
            array('name' => 'Mississippi', 'abbr' => 'MS'),
            array('name' => 'Missouri', 'abbr' => 'MO'),
            array('name' => 'Montana', 'abbr' => 'MT'),
            array('name' => 'Nebraska', 'abbr' => 'NE'),
            array('name' => 'Nevada', 'abbr' => 'NV'),
            array('name' => 'New Hampshire', 'abbr' => 'NH'),
            array('name' => 'New Jersey', 'abbr' => 'NJ'),
            array('name' => 'New Mexico', 'abbr' => 'NM'),
            array('name' => 'New York', 'abbr' => 'NY'),
            array('name' => 'North Carolina', 'abbr' => 'NC'),
            array('name' => 'North Dakota', 'abbr' => 'ND'),
            array('name' => 'Ohio', 'abbr' => 'OH'),
            array('name' => 'Oklahoma', 'abbr' => 'OK'),
            array('name' => 'Oregon', 'abbr' => 'OR'),
            array('name' => 'Pennsylvania', 'abbr' => 'PA'),
            array('name' => 'Rhode Island', 'abbr' => 'RI'),
            array('name' => 'South Carolina', 'abbr' => 'SC'),
            array('name' => 'South Dakota', 'abbr' => 'SD'),
            array('name' => 'Tennessee', 'abbr' => 'TN'),
            array('name' => 'Texas', 'abbr' => 'TX'),
            array('name' => 'Utah', 'abbr' => 'UT'),
            array('name' => 'Vermont', 'abbr' => 'VT'),
            array('name' => 'Virginia', 'abbr' => 'VA'),
            array('name' => 'Washington', 'abbr' => 'WA'),
            array('name' => 'West Virginia', 'abbr' => 'WV'),
            array('name' => 'Wisconsin', 'abbr' => 'WI'),
            array('name' => 'Wyoming', 'abbr' => 'WY'),
            array('name' => 'Virgin Islands', 'abbr' => 'V.I.'),
            array('name' => 'Guam', 'abbr' => 'GU'),
            array('name' => 'Puerto Rico', 'abbr' => 'PR')
        );

        $return = false;
        $strlen = strlen($name);

        foreach ($states as $state) :
            if ($strlen < 2) {
                return false;
            } else if ($strlen == 2) {
                if (strtolower($state['abbr']) == strtolower($name)) {
                    $return = $state['name'];
                    break;
                }
            } else {
                if (strtolower($state['name']) == strtolower($name)) {
                    $return = strtoupper($state['abbr']);
                    break;
                }
            }
        endforeach;

        return $return;
    }
}
