import { useState } from 'react';
import { Image } from 'react-bootstrap';

import { AvatarByRole } from '@enums';
import { nonEmployeeIcon } from '@icons';

import styles from './avatar.scss';

type AvatarProps = {
  src?: string;
  alt?: string;
  width?: number;
  role?: AvatarByRole;
  hasBorder?: boolean;
}

const INITIALS_LENGTH = 2;

const getInitials = (nameString: string) =>
  nameString
    .split(' ')
    .slice(0, INITIALS_LENGTH)
    .map(name =>
      name[0])
    .join('')
    .toUpperCase();

export const Avatar = ({
  src,
  alt = 'avatar',
  width = 1.5,
  role = AvatarByRole.REGULAR_EMPLOYEE,
  hasBorder = false,
}: AvatarProps) => {
  const [ error, setError ] = useState<boolean>(false);
  const [ loaded, setLoaded ] = useState<boolean>(false);
  const isImageShown = src && !error;

  const avatarClassName = []
    .concat(hasBorder ? styles[`borderAvatar${role}`] : '')
    .concat(styles.avatar)
    .join(' ');

  const letteredClassName = []
    .concat(styles.avatarLettered)
    .concat(loaded ? styles.avatarLetteredTransparent : styles[`avatarLettered${role}`])
    .join(' ');

  return (
    <div
      className={avatarClassName}
      style={{
        width: `${width}rem`,
        height: `${width}rem`,
        position: 'relative',
        fontSize: `${width / 3}rem`,
        borderWidth: `${width * 0.02}rem`,
        outlineWidth: `${width * 0.03}rem`,
      }}
    >
      {isImageShown && (
        <Image
          src={src}
          alt={alt}
          title={alt}
          width="100%"
          height="100%"
          roundedCircle
          onLoad={() =>
            setLoaded(true)}
          onError={() =>
            setError(true)}
        />
      )}
      {(!loaded || error) && (
        role === AvatarByRole.NON_EMPLOYEE
          ? (
            <Image
              className={styles.avatarFallback}
              src={nonEmployeeIcon}
              alt={alt}
              title={alt}
              roundedCircle
            />
          )
          : (
            <div className={letteredClassName}>
              {!isImageShown && getInitials(alt)}
            </div>
          )
      )}
    </div>
  );
};
