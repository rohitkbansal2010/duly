import { PropsWithChildren } from 'react';
import { PlaceholderAnimation } from 'react-bootstrap/esm/usePlaceholder';
import Placeholder from 'react-bootstrap/Placeholder';

type SkeletonProps = {
  width?: string,
  height?: string,
  borderRadius?: string,
  animation?: PlaceholderAnimation,
  background?: string,
  style?: React.CSSProperties;
}

export const Skeleton = ({
  width = '100%',
  height = 'auto',
  background = '#cfd0d1',
  borderRadius = '1rem',
  animation,
  style,
  children,
}: PropsWithChildren<SkeletonProps>) => 
  (
    <Placeholder
      as="div"
      style={{
        ...style,
        width,
        height,
        background,
        borderRadius,
      }}
      animation={animation}
    >{children}</Placeholder>
  );
