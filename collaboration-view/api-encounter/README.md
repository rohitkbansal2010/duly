[![Build Status](https://dev.azure.com/Next-Generation-Data-Platform/duly.app.v1.collaboration-view/_apis/build/status/api-encounter-webapi?repoName=api-encounter&branchName=dev)](https://dev.azure.com/Next-Generation-Data-Platform/duly.app.v1.collaboration-view/_build/latest?definitionId=15&repoName=api-encounter&branchName=dev)

[![Quality Gate Status](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=alert_status&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Reliability Rating](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=reliability_rating&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Security Rating](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=security_rating&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)

[![Bugs](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=bugs&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Lines of Code](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=ncloc&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Coverage](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=coverage&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Vulnerabilities](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=vulnerabilities&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)
[![Code Smells](https://sonarqube.duly-np.digital/api/project_badges/measure?project=duly_duly.app.v1.collaboration-view_api-encounter_webapi&metric=code_smells&token=2643b544e41f0dd12d1e54cd91d01034acc77362)](https://sonarqube.duly-np.digital/dashboard?id=duly_duly.app.v1.collaboration-view_api-encounter_webapi)

# Introduction 
Encounter Api provides backend for collaboration view. It gets all data from all System API's that are required.

It consits of:
- [src/WebApi](./src/WebApi) - Provides WebApi for [Front End](https://dev.azure.com/Next-Generation-Data-Platform/duly.app.v1.collaboration-view/_git/web-views).

# Getting Started 
Using Visual Studio 2019:
1.  [Install Visual Studio 2019](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2019) 
2.	Clone the Repo using your favorite method. This can be done either through ADO or through ssh
3.	Open solution in visual studio src/Duly.CollaborationView.Api.Encounter.sln

# Build and Test
Using Visual Studio 2019:
All projects are configured to work out of the box, so just running Build All and Run All Tests Should do the trick.

# Auth
Web API requires authentithication to yield any results.
To authentithicate follow these steps:
1. Click on Authorize 🔓
2. Paste client Id which can be provided by Systems Engineer or Developer Team Lead
3. Check all required scopes
4. Press Authorize 
5. You will be taken to organisation login page. Check that your Duly account is sellected
6. Compelte all needed authorization steps on microsoft portal as usual
7. In the end you should be forwarded back to the Swagger and see Authorized
8. Press Close
9. Enjoy using the API