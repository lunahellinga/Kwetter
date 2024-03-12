import {Kweet} from './kweet';

describe('Kweet', () => {
  it('should create an instance', () => {
    expect(new Kweet(0, "", "", new Date(Date.now()), ['blessed'], ['Luna'])).toBeTruthy();
  });
});
