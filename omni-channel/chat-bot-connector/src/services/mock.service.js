'use strict';

const mockData = {
  cancelAppointment: {
    status: 200,
    body: {
      success: true,
    }
  },
  getAppointments: {
    status: 200,
    body: {
      success: true,
      appointments: [
        {
          id: 10000808792,
          date: '2021-08-21',
          time: '1:00 PM',
          duration: 20,
          provider: 'Leslie Rumack',
          visitType: 'Sample Visit 1',
        },
        {
          id: 10000808799,
          date: '2021-08-28',
          time: '10:00 AM',
          duration: 30,
          provider: 'John Smith',
          visitType: 'Sample Visit 2',
        }
      ]
    }
  },
};

module.exports = { mockData };