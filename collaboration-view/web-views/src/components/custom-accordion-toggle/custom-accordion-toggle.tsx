import { useContext } from 'react';
import { AccordionContext, useAccordionButton } from 'react-bootstrap';

type CustomAccordionTogglePropsType = {
  eventKey: string;
  callback: (isCurrentEventKey: boolean) => void;
  children: (
    decoratedOnClick: ReturnType<typeof useAccordionButton>,
    isCurrentEventKey: boolean
  ) => JSX.Element;
};

export const CustomAccordionToggle = ({
  eventKey,
  callback,
  children,
}: CustomAccordionTogglePropsType) => {
  const { activeEventKey } = useContext(AccordionContext);

  const isCurrentEventKey = activeEventKey === eventKey;

  const decoratedOnClick = useAccordionButton(
    eventKey,
    () =>
      callback(isCurrentEventKey)
  );

  return children(decoratedOnClick, isCurrentEventKey);
};
