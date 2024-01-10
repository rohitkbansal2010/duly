/* eslint-disable no-console */

import { string } from 'joi';
import { TriggerEvent } from './trigger';

const LOG_MAX_LENGTH = 480;

const breakMessageOnParts = (val: any): string[] => {
  let msg = typeof val === 'string' ? val : JSON.stringify(val);
  const parts = [];
  while (msg.length) {
    parts.push(msg.slice(0, LOG_MAX_LENGTH));
    msg = msg.slice(LOG_MAX_LENGTH);
  }
  return parts;
};

export default class Logger {
  executionId: string | undefined;

  constructor(executionId: string) {
    this.executionId = executionId;
  }

  log(...args: any[]) {
    const msg = args.reduce((acc, val) => {
      const valMsg = typeof val === 'string' ? val : JSON.stringify(val);
      return `${acc}${acc.length ? ', ' : ''}${valMsg}`;
    }, '');
    breakMessageOnParts(msg).forEach((part) => {
      console.log(`${this.executionId || '-'}`, part);
    });
  }

  warn(message: any) {
    breakMessageOnParts(typeof message === 'string' ? message : JSON.stringify(message))
      .forEach((part) => {
        console.warn(`${this.executionId || '-'}`, part);
      });
  }

  info(message: any) {
    breakMessageOnParts(typeof message === 'string' ? message : JSON.stringify(message))
      .forEach((part) => {
        console.info(`${this.executionId || '-'}`, part);
      });
  }

  triggerEvent(event: TriggerEvent<unknown>) {
    const { to = '-', correlationToken = '-', parameters = {} } = event;
    this.log(`TRIGGER ${to} (${correlationToken}): ${JSON.stringify(parameters)}`);
  }
}
