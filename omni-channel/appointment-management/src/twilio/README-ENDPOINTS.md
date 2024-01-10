# TOKEN VERIFICATION

Will verify a token for the Twilio Trigger invocation

https://duly-communication-hub-6963.twil.io/token-verification
https://duly-communication-hub-6963-dev.twil.io/token-verification

```
{
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJEdWx5LUNvbUh1YiIsImF1ZCI6IlR3aWxpbyIsInBlcm1zIjpbInRyaWdnZXIxIl0sImlhdCI6MTY0NDg3MDM2MSwiZXhwIjoxNjQ0OTU2NzYxfQ.__jpIxtVEXMuH5w94N0DZsKW0cAvtSnRC5TrRuoDKg1o"
}
```


# COMMUNICATION HUB CONNECTION

https://duly-communication-hub-6963.twil.io/ping-communication-hub
https://duly-communication-hub-6963-dev.twil.io/ping-communication-hub

```
{
    "correlationToken": "{{correlationToken}}",
    "meta": "test"
}
```


# INVITATION

https://duly-communication-hub-6963.twil.io/appointment-management-invitation
https://duly-communication-hub-6963-dev.twil.io/appointment-management-invitation

```
{
    "to": "+18443296795",
    "correlationToken": "{{correlationToken}}",
    "parameters": {
        "patientName": "Anna",
        "micrositeServicesUrl": "https://dev-invitation.link"
    },
    "callbackUrl": "http://178.124.162.146:1337/log"
}
```



# CONFIRMATION

https://duly-communication-hub-6963.twil.io/appointment-management-confirmation
https://duly-communication-hub-6963-dev.twil.io/appointment-management-confirmation

```
{
    "to": "+18443296795",
    "correlationToken": "{{correlationToken}}",
    "parameters": {
        "providerName": "Dr. Mary Codo",
        "confirmationPageUrl": "https://dev-confirmation.link",
        "departmentName": "Duly Health and Care Oak Park",
        "appointmentDateTime": "2022-03-21T13:45:30",
        "streetName": "1133 South Blvd",
        "city": "Oak Park",
        "state": "IL",
        "zipCode": "60302"
    },
    "callbackUrl": "http://178.124.162.146:1337/log"
}
```
