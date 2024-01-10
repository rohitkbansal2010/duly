import { SyncMapItemInstance } from 'twilio/lib/rest/sync/v1/service/syncMap/syncMapItem';
import { SyncMapContext } from 'twilio/lib/rest/sync/v1/service/syncMap';
import SyncMapData, { StageData } from './syncMapData';

const SYNC_MAP_ITEM_TTL = 0; // should be 0 for permanent data storing
const ERROR_CODE_FOR_FAILURE = 20404;
const STAGE_TTL = 2_592_000; // time to store stage in seconds
const MIN_NUMBER_OF_STAGES_TO_STORE = 10; // minimum number of stages to store
const MAX_SIZE_IN_BYTES = 16_000;

function item2data(item: SyncMapItemInstance): SyncMapData {
  const { data, key } = item;
  const mapData = new SyncMapData(key);
  mapData.optout = data.optout;
  mapData.stages = (data.stages || []).map((values: any | undefined) => ({
    stage: values?.stage || 'UNKNOWN',
    datetime: values?.datetime ? new Date(values?.datetime) : new Date(),
    correlationToken: values?.correlationToken || 'UNKNOWN',
    parameters: values?.parameters || {},
    callbackUrl: values?.callbackUrl || undefined,
  }));
  return mapData;
}

export default class SyncMapDataAdapter {
  context: SyncMapContext;

  constructor(syncMap: SyncMapContext) {
    this.context = syncMap;
  }

  async save(mapData: SyncMapData): Promise<SyncMapData> {
    const { key, stages, optout } = mapData;
    const data = { optout, stages: [] as StageData[] };
    // GATHER STAGES
    let approximateSizeInBytes = 0;
    const dateFrom = new Date(new Date().getTime() - STAGE_TTL * 1e3);
    for (let i = 0; i < stages.length; i += 1) {
      const stage = stages[i];
      approximateSizeInBytes += Buffer.byteLength(JSON.stringify(stage));
      if (approximateSizeInBytes > MAX_SIZE_IN_BYTES) {
        break;
      }
      if (i >= MIN_NUMBER_OF_STAGES_TO_STORE && stage.datetime < dateFrom) {
        break;
      }
      data.stages.push(stage);
    }
    // SAVE MAP ITEM
    try {
      const item = await this.context.syncMapItems(key).update({ data, ttl: SYNC_MAP_ITEM_TTL });
      return item2data(item);
    } catch (error) {
      if (error.code === ERROR_CODE_FOR_FAILURE) {
        const item = await this.context.syncMapItems.create({ key, data, ttl: SYNC_MAP_ITEM_TTL });
        return item2data(item);
      }
      throw error;
    }
  }

  async getOrCreate(key: string): Promise<SyncMapData> {
    try {
      const item = await this.context.syncMapItems(key).fetch();
      const { dateExpires } = item;
      if (!dateExpires || dateExpires < new Date()) {
        return item2data(item);
      }
      return new SyncMapData(key);
    } catch (error) {
      if (error.code === ERROR_CODE_FOR_FAILURE) {
        return new SyncMapData(key);
      }
      throw error;
    }
  }
}
