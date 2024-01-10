# Introduction 
The OAuth 2.0 core specification [RFC6749](https://tools.ietf.org/html/rfc6749) defines several
ways for a client to obtain refresh and access tokens. JSON Web Tokens (JWT) is a way of
statelessly handling user authentication, which helps to organize authentication without storing
the authentication state in any storage be it a session or a database. As result, whenever a
token is created, it can be used forever, or until it is expired.

The specification [RFC7009](https://tools.ietf.org/html/rfc7009) proposes an additional endpoint
for OAuth authorization servers, which allows clients to notify the authorization server that a
previously obtained refresh or access token is no longer needed. However only the revocation of
refresh tokens must be supported by authorization servers, but the revocation of access tokens
is optional. As result, at present time Microsoft Identity Platform does not provide the way how
an access token can be explicitly revoked / invalidated (look at https://docs.microsoft.com/en-us/azure/active-directory/develop/access-tokens#token-revocation).

The invalidation approach of JWT access token is based on configuration of Azure API Management
(APIM) inbound policies and preferably external Azure Redis Cache server.

# Prerequisite

- Create an instance of Azure Cache for Redis and register it inside APIM as external cache;
- Create a new blank API endpoint "enterprise-security" (use "shared/security" as value of API URL
suffix parameter) and register new operation (or alternatively use "specificaton.json" to
import API defention):

|Display Name    |Name                 |Description  |URL            |Response |
|:---------------|:--------------------|:------------|:--------------|:--------|
|InvalidateToken |invalidate-token     |Invalidate JWT to avoid re-usage after logout|type POST<br/>value "/invalidate"|200 OK|

# Configure
- Edit inbound processing policiy of the created operation above, apply the XML from
"operation-policy.xml";
- On the top level of the API (All operations), apply the inbound processing XML from
"api-policy.xml".

# Consume
Adjust XML of the inbound processing policy for consuming back-end API, apply the code snippet
from "consuming-policy.xml".