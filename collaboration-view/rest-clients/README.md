# Introduction
This repository contains postman collection for using Epic APIs.

Client certificates needs to be configured in Postman for mTLS Authentication. Certificates are present in Azure Key Vaults


|Environment|KeyVault Name|Certificate Name| Certificate Use
|--|--|--|--|
| Non-Prod | duly-d-certs-kv | digital-d-clearstep-cert | Access Clearstep APIs |
| Non-Prod | duly-d-certs-kv | digital-d-epic-bridge-cert | Access Epic Bridge APIs |
| Prod | duly-p-certs-kv | digital-p-clearstep-cert | Access Clearstep APIs|
| Prod | duly-p-certs-kv | digital-p-epic-bridge-cert | Access Epic Bridge APIs |
