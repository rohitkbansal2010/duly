module.exports = {

  default: `You’re all set!
<%= providerName %> will see you on <%= formatAppointmentDateAndTime(appointmentDateTime) %> CT.

<%= formatAddress(streetName, city, state, zipCode) %>

Click here for details on your upcoming appointment:
<%= confirmationPageUrl %>
`,

};
