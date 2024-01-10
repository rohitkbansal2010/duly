import { Photo } from '@types';

export const getSrcAvatar = (photo?: Photo): string => {
  if (!photo) {
    return '';
  }

  const {
    contentType,
    data,
    url,
  } = photo;

  const photoUrl = url ? url : '';

  return data ? `data:${contentType};base64,${data}` : photoUrl;
};
