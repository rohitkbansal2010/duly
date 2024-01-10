<?php

namespace modules\physiciansmodule\exporters;

use Craft;
use craft\base\EagerLoadingFieldInterface;
use craft\base\Element;
use craft\base\ElementExporter;
use craft\base\Field;
use craft\db\Query;
use craft\elements\Asset;
use craft\elements\db\ElementQuery;
use craft\elements\db\ElementQueryInterface;
use craft\elements\Entry;
use craft\helpers\ArrayHelper;
use modules\DupageCoreModule\DupageCoreModule;
use yii\db\Expression;
use yii\helpers\Inflector;

class PhysicianExporter extends ElementExporter
{
    public static function displayName(): string
    {
        return 'Custom Physician Exporter';
    }

    public function export(ElementQueryInterface $query = null): array
    {
        $results = [];

        if ($query) {
            $physicianIds = $query->ids();
        } else {
            $physicianIds = Entry::find()
            ->section('physicians')
            ->ids();
        }

        $siteUrl = getenv('DEFAULT_SITE_URL');

        $physicians = (new Query)
            ->select([
                'c.id',
                'c.elementId',
                'c.title',
                'field_firstname AS firstName',
                'field_lastname AS lastName',
                'field_epicProviderId',
                new Expression("JSON_UNQUOTE(JSON_EXTRACT(c.field_physicianCellPhone, '$.number')) AS cellPhone"),
                'field_physicianEmail',
                'field_residency_residencyLocation AS residency',
                'field_restriction_minimumAge AS minAge',
                'field_restriction_maximumAge AS maxAge',
                'field_education_schoolName AS medicalSchool',
                'field_fellowships_fellowshipLocation AS fellowship',
                'field_internship_internshipSchoolName AS internship',
                'field_allowsOnlineScheduling AS allowOnlineScheduling',
                'et.handle AS category',
                'field_gender AS gender',
                'field_nationalProviderIdentifier as npi',
                'field_physicianRegion AS regions',
                new Expression("GROUP_CONCAT(distinct conditionsSeenQuery.title SEPARATOR '\n') as conditionsSeen"),
                new Expression("GROUP_CONCAT(distinct proceduresPerformedQuery.title SEPARATOR '\n') as procedures"),
                new Expression("GROUP_CONCAT(distinct physicianHospitalAffiliationsQuery.title SEPARATOR '\n') as hospitals"),
                new Expression("GROUP_CONCAT(distinct physicianSpecialityQuery.title SEPARATOR '\n') as specialties"),
                new Expression("GROUP_CONCAT(distinct languagesSpokenQuery.title SEPARATOR '\n') as languages"),
                'physicianHeadshotQuery.filename as headshot',
            ])
            ->from('content AS c')
            ->join('left join', 'elements e', 'c.elementId = e.id')
            ->join('left join', 'entrytypes et', 'e.fieldLayoutId = et.fieldLayoutId')
            ->where(['in', 'c.elementId', $physicianIds])
            ->groupBy('c.elementId')
            ->orderBy('title ASC');

        $this->addMatrixRelation($physicians, 'Residency', ['ownerId', 'field_residency_residencyLocation'], 'matrixcontent_residency');
        $this->addMatrixRelation($physicians, 'Age Restriction', ['ownerId', 'field_restriction_minimumAge', 'field_restriction_maximumAge'], 'matrixcontent_agerestrictions');
        $this->addMatrixRelation($physicians, 'Education', ['ownerId', 'field_education_schoolName'], 'matrixcontent_physicianeducation');
        $this->addMatrixRelation($physicians, 'Fellowships', ['ownerId', 'field_fellowships_fellowshipLocation'], 'matrixcontent_fellowships');
        $this->addMatrixRelation($physicians, 'Internship', ['ownerId', 'field_internship_internshipSchoolName'], 'matrixcontent_internships');
        $this->getFieldAssociatedContentQuery($physicians, 'conditionsSeen');
        $this->getFieldAssociatedContentQuery($physicians, 'proceduresPerformed');
        $this->getFieldAssociatedContentQuery($physicians, 'physicianHospitalAffiliations');
        $this->getFieldAssociatedContentQuery($physicians, 'physicianSpeciality');
        $this->getFieldAssociatedContentQuery($physicians, 'languagesSpoken');
        $this->getHeadshotQuery($physicians, 'physicianHeadshot');

        // DMG-2614
        // Dec 7, 2021: adjusted the batch size from (default) 100 to 10
        // in an attempt to help this function execute successfully and on time.
        foreach ($physicians->each(10) as $physician) {
            $ageMin = $physician['minAge'] ?? 0;
            $ageMax = $physician['maxAge'] ?? 999;
            $title = $physician['title'];
            $headshotUrl = '';

            $entry = Entry::find()
                ->section('physicians')
                ->select(['uri'])
                ->where(['elements.id' => $physician['elementId']])
                ->one();

            if ($physician['headshot']) {
                $imageAsset = Asset::find()
                    ->filename($physician['headshot'])
                    ->one();

                if ($imageAsset) {
                    $headshotUrl = DupageCoreModule::getInstance()
                        ->dupageCoreModuleService
                        ->getOptimizedImage(
                            $imageAsset,
                            'jpeg',
                            true,
                            [
                                [ 'settings' => [ 'resize:auto' ] ]
                            ]
                        )[0];
                }
            }

            $results[] = [
                'Name' => $title ?? '',
                'First Name' => $physician['firstName'] ?? '',
                'Last Name' => $physician['lastName'] ?? '',
                'Profile URL' => $entry !== null ? ($entry->getUrl() ?? '') : '',
                'Epic Provider ID' => $physician['field_epicProviderId'] ?? '',
                'NPI' => $physician['npi'] ?? '',
                'Gender' =>  $physician['gender'] ?? '',
                'Category' =>  $physician['category'] ?? '',
                'Cell Phone' => $physician['cellPhone'] ?? '',
                'Email' => $physician['field_physicianEmail'] ?? '',
                "Age's Seen" => "{$ageMin} - {$ageMax}",
                'Online Scheduling' =>  $physician['allowOnlineScheduling'] == 1 ? 'Yes' : 'No',
                'Regions' => $physician['regions'] ?? '',
                'Residency' =>  $physician['residency'] ?? '',
                'Languages Spoken' =>  $physician['languages'] ?? '',
                'Specialties' =>  $physician['specialties'] ?? '',
                'Procedures' =>  $physician['procedures'] ?? '',
                'Conditions Seen' =>  $physician['conditionsSeen'] ?? '',
                'Hospital Affliations' =>  $physician['hospitals'] ?? '',
                'Medical Schools' =>  $physician['medicalSchool'] ?? '',
                'Fellowships' =>  $physician['fellowship'] ?? '',
                'Internships' =>  $physician['internship'] ?? '',
                'Physician Headshot URL' =>  $headshotUrl,
            ];
        }

        return $results;
    }

    private function getFieldAssociatedContentQuery(Query $physicians, string $fieldHandle)
    {
        $fieldByHandleQuery = (new Query)
            ->select('id')
            ->from('fields')
            ->where(['handle' => $fieldHandle]);

        $entryQuery = (new Query)
            ->select([
                'content.title',
                'relations.sourceId',
            ])
            ->from('content')
            ->innerJoin('relations', 'content.elementId = relations.targetId')
            ->innerJoin(['fieldByHandleQuery' => $fieldByHandleQuery], 'fieldByHandleQuery.id = relations.fieldId');

        $physicians->leftJoin(["{$fieldHandle}Query" => $entryQuery], "{$fieldHandle}Query.sourceId = c.elementId");
    }

    private function getHeadshotQuery(Query $physicians, string $fieldHandle)
    {
        $fieldByHandleQuery = (new Query)
            ->select('id')
            ->from('fields')
            ->where(['handle' => $fieldHandle]);

        $entryQuery = (new Query)
            ->select([
                'filename',
                'relations.sourceId',
            ])
            ->from('assets')
            ->innerJoin('relations', 'assets.id = relations.targetId')
            ->innerJoin(['fieldByHandleQuery' => $fieldByHandleQuery], 'fieldByHandleQuery.id = relations.fieldId');

        $physicians->leftJoin(["{$fieldHandle}Query" => $entryQuery], "{$fieldHandle}Query.sourceId = c.elementId");
    }

    private function addMatrixRelation(Query $physicians, string $blockTypeName, array $fields, string $matrixName)
    {
        $matrixBlockTypeId = (new Query)
            ->select('id')
            ->from('matrixblocktypes')
            ->where(['name' => $blockTypeName])
            ->one()
            ['id'];
    
        $relatedMatrixQuery = (new Query)
            ->select($fields)
            ->from($matrixName)
            ->innerJoin('matrixblocks', "matrixblocks.id = {$matrixName}.elementId")
            ->where([
                'typeId' => $matrixBlockTypeId
            ]);

        $physicians->leftJoin(["{$matrixName}relatedMatrixQuery" => $relatedMatrixQuery], "{$matrixName}relatedMatrixQuery.ownerId = c.elementId");
    }
}
