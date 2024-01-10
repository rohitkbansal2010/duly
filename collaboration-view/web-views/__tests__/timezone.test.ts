describe('getUserTimeZoneOrOffset', () => {
  beforeEach(() => {
    jest
      .spyOn(Date.prototype, 'getTimezoneOffset')
      .mockImplementation(() => 
        0);
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it('should be UTC', () => {
    expect(new Date().getTimezoneOffset()).toBe(0);
  });
});
