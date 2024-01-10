import { useEffect, useState } from 'react';
import {
  Col, Form, Nav, Row
} from 'react-bootstrap';
import { useDispatch, useSelector } from 'react-redux';

import { FontIconBlue, FontIconSkyblue } from '@icons';
import { setFontSize } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';

import styles from './font-size-setting.scss';
import fontSizes from './font-size.json';
import { FONT_SMALL, FONT_LARGE, FONT_MEDIUM } from './helper';

function FontSizeSetting() {
  const root = document.documentElement;
  const style = root?.style;
  const activeKey = useSelector(({ APPSTATE }: RootState) =>
    APPSTATE?.fontSize);
  const dispatch: AppDispatch = useDispatch();

  const [ range, setRange ] = useState('1');

  const smallSize = () => {
    setRange('1');
    for (const fontSize of fontSizes) {
      style.setProperty(fontSize.name, fontSize.small);
    }
  };

  const mediumSize = () => {
    setRange('2');
    for (const fontSize of fontSizes) {
      style.setProperty(fontSize.name, fontSize.medium);
    }
  };

  const largeSize = () => {
    setRange('3');
    for (const fontSize of fontSizes) {
      style.setProperty(fontSize.name, fontSize.large);
    }
  };
  const setActiveKeyOnChange = (key: string) => {
    if(dispatch)
      dispatch(setFontSize(key));
    switch (key) {
      case FONT_SMALL:
        smallSize();
        break;
      case FONT_MEDIUM:
        mediumSize();
        break;
      case FONT_LARGE:
        largeSize();
        break;
    }
  };

  const setRangeOnchange = (key: string) => {
    switch (key) {
      case '1':
        setActiveKeyOnChange(FONT_SMALL);
        break;
      case '2':
        setActiveKeyOnChange(FONT_MEDIUM);
        break;
      case '3':
        setActiveKeyOnChange(FONT_LARGE);
        break;
    }
  };

  useEffect(() => {
    setActiveKeyOnChange(activeKey);
  }, []);

  return (
    <>
      <div className={styles.fontSetting}>
        <Row>
          <Col>
            <div className={styles.fontSizes}>

              <Nav defaultActiveKey={FONT_SMALL} onSelect={setActiveKeyOnChange}>
                <Nav.Item>
                  <Nav.Link eventKey={FONT_SMALL}>
                    <div className={styles.fontSizeSmall}>
                      <img
                        src={activeKey === FONT_SMALL ?
                          FontIconSkyblue : FontIconBlue}
                        alt={FONT_SMALL}
                      />
                    </div>
                  </Nav.Link>
                </Nav.Item>
                <Nav.Item>
                  <Nav.Link eventKey={FONT_MEDIUM}>
                    <div className={styles.fontSizeMedium}>
                      <img
                        src={activeKey === FONT_MEDIUM ?
                          FontIconSkyblue : FontIconBlue}
                        alt={FONT_MEDIUM}
                      />
                    </div>
                  </Nav.Link>
                </Nav.Item>
                <Nav.Item>
                  <Nav.Link eventKey={FONT_LARGE} >
                    <div className={styles.fontSizeLarge}>
                      <img
                        src={activeKey === FONT_LARGE ?
                          FontIconSkyblue : FontIconBlue}
                        alt={FONT_LARGE}
                      />
                    </div>
                  </Nav.Link>
                </Nav.Item>
              </Nav>
            </div>
          </Col>
        </Row>
        <Row>
          <Col>
            <Form.Range
              min={'1'}
              max={'3'}
              step={'1'}
              value={range}
              className={styles.formRange}
              onChange={event =>
                setRangeOnchange(event.target.value)}
            />
          </Col></Row>
      </div>
    </>
  );
}

export default FontSizeSetting;
