export type StageData = {
    stage: string,
    datetime: Date,
    correlationToken: string,
    parameters: Record<string, string>,
    callbackUrl?: string
}

export default class SyncMapData {
  key: string;

  optout: boolean = false;

  stages: StageData[] = [];

  constructor(key: string) {
    this.key = key;
    this.optout = false;
    this.stages = [];
  }

  hasData(idx: number = 0) {
    return idx < this.stages.length;
  }

  getData(idx: number = 0) {
    if (this.stages.length <= idx) {
      return null;
    }
    if (idx < 0) {
      throw new Error('Wrong stage index');
    }
    return this.stages[idx];
  }

  addData(stage: string, correlationToken: string, parameters: StageData['parameters'], callbackUrl?: string) {
    this.stages.unshift({
      stage,
      datetime: new Date(),
      correlationToken,
      parameters,
      callbackUrl,
    });
  }

  updateData(stage?: string, correlationToken?: string, parameters?: StageData['parameters'], callbackUrl?: string) {
    const lastData = this.getData();
    if (!lastData) {
      throw new Error('There is no data in the syncMapItem');
    }
    this.stages.unshift({
      stage: stage || lastData.stage,
      datetime: new Date(),
      correlationToken: correlationToken || lastData.correlationToken,
      parameters: parameters || lastData.parameters,
      callbackUrl: callbackUrl || lastData.callbackUrl,
    });
    return this;
  }
}
