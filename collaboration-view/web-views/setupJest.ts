import 'jsdom-global/register';
import Adapter from '@wojtekmaj/enzyme-adapter-react-17';
import { configure } from 'enzyme';

import crypto from 'crypto';

(window as any).env = {
  WORK_DAY_START: '7',
  WORK_DAY_END:'19',
};

const mutationObserverMock = jest.fn(
  function MutationObserver(callback) {
    this.observe = jest.fn();
    this.disconnect = jest.fn();
    this.trigger = (mockedMutationsList) => {
      callback(mockedMutationsList, this);
    };
  }
);

configure({ adapter: new Adapter() });

window.crypto = crypto;
global.MutationObserver = mutationObserverMock;
