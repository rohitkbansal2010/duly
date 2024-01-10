
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { noop } from 'lodash';
import moment from 'moment';
import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';

import { AfterStepsAccordian, CardDetailsType } from '@components/cv-checkout-accordian/after-steps-accordian';
import { DulyLoader } from '@components/duly-loader';
import { ExportPdf, Line, nonEmployeePngIcon } from '@icons';
import {
  followUpHeaderData,
  imagingHeaderData,
  labsHeaderData,
  referralHeaderData
} from '@mock-data';
import { RootState } from '@redux/reducers';
import { ProviderDetailsType } from '@types';
import {
  firstLetterCapital,
  formatAddressBottomLine,
  formatDateStringAddComma,
  calculateYears,
  getSrcAvatar,
  writeHumanName
} from '@utils';

import 'bootstrap-icons/font/bootstrap-icons.css';

import styles from './accordian.scss';
import { PrescriptionAccordian } from './prescriptionAccordian/prescription-accordian';

const defaultAccordionState = {
  followUp: '0',
  labs: '0',
  referral: '0',
  prescription: '0',
};

export const AfterVisitSteps = () => {
  const [ allAccordionOpen, setAllAccordionOpen ] = useState(defaultAccordionState);
  const getFollowUpAppointmentLocation = (location: string) => {
    const locationList = location.split(', ');
    const locations = locationList[0];
    const pincode = `${locationList[1]}, ${locationList[2]} ${locationList[3]}`;

    return {
      location: locations,
      pincode: pincode,
    };
  };

  const [ referralCardData, setReferralCardData ] = useState<CardDetailsType[]>([]);
  const [ imagingCardData, setImagingCardData ] = useState<CardDetailsType[]>([]);

  const [ pdfExportStatusText, setPdfExportStatusText ] = useState('Generating PDF...');
  const [ isPdfExporting, setIsPdfExporting ] = useState(false);
  const patientData = useSelector(({ CURRENT_APPOINTMENT }: RootState) =>
    CURRENT_APPOINTMENT.patientData);
  const {
    scheduledFollowUpDetails,
    followUpOrderDetails,
    scheduledLabTestDetails,
    referralDetails,
    scheduledReferralDetails,
    isFollowUpScheduled,
    isLabTestScheduled,
    labTestDetails,
    scheduledImagingTestDetails,
    imagingTestDetails,
  } = useSelector(({ CHECKOUT_APPOINTMENTS }: RootState) =>
    CHECKOUT_APPOINTMENTS);

  const prescriptionData = useSelector(
    ({ OVERVIEW_WIDGETS }: RootState) =>
      OVERVIEW_WIDGETS.medications
  );

  const creatAppoinmentCard =
    async (pdf: any, cards: any, yAxis: any, PageHeight: any, imgWidth: any, imgHeight: any) => {
      for (const element of cards) {
        const canvas = await html2canvas(element, {
          useCORS: true,
          allowTaint: true,
        });
        const pageData = canvas.toDataURL('image/png', {
          scale: 3,
          optimized: false,
        });

        let height = 45;
        if (element.querySelectorAll('[class*=\'cardHeaderDate\']')?.length > 0 || element.querySelectorAll('[class*=\'cardHeaderFlex\']')?.length > 0) {
          if (element?.querySelectorAll('.pincode-line')?.length > 0) {
            height = 60;
          } else {
            height = 52;
          }
        }

        height = imgHeight ? imgHeight : height;

        if (yAxis > (PageHeight - height)) {
          pdf.addPage();
          yAxis = 20;
        }

        pdf.addImage(pageData, 'PNG', 10, yAxis, imgWidth, height);
        yAxis = yAxis + height + 10;
      }
      return yAxis;
    };

  const printPrescription =
    async (pdf: any, topDiv: any, yAxis: any, PageHeight: any, imgWidth: any) => {
      if (topDiv.querySelectorAll('.prescription-container')) {
        let cardYAxis = yAxis;
        let prescriptionMapYAxis = yAxis;
        const prescriptionCards = topDiv.querySelectorAll('.prescription-card');
        if (prescriptionCards?.length > 0) {
          cardYAxis = await creatAppoinmentCard(
            pdf,
            prescriptionCards,
            cardYAxis,
            PageHeight,
            (imgWidth / 2) - 10,
            72
          );
        }
        let isOnPreviousPage = false;
        if (cardYAxis < yAxis) {
          pdf.setPage(pdf.internal.getNumberOfPages() - 1);
          isOnPreviousPage = true;
        }
        const prescriptionMap = topDiv.querySelector('#prescription-accordian-map');
        if (prescriptionMap) {
          const canvas = await html2canvas(prescriptionMap, {
            useCORS: true,
            allowTaint: true,
          });
          const pageData = canvas.toDataURL('image/png');


          const prescriptionHeight = 140;
          if (prescriptionMapYAxis > (PageHeight - prescriptionHeight)) {
            if (isOnPreviousPage) {
              pdf.setPage(pdf.internal.getNumberOfPages());
            } else {
              pdf.addPage();
            }
            prescriptionMapYAxis = 20;
          }

          pdf.addImage(pageData, 'PNG', (imgWidth / 2) + 15, prescriptionMapYAxis, (imgWidth / 2) - 15, prescriptionHeight);
          prescriptionMapYAxis = prescriptionMapYAxis + prescriptionHeight + 10;
        }

        const prescriptionMapdetail = topDiv.querySelector('.prescription-map-detail');
        if (prescriptionMapdetail) {
          const canvas = await html2canvas(prescriptionMapdetail, {
            useCORS: true,
            allowTaint: false,
          });
          const pageData = canvas.toDataURL('image/png');


          const prescriptionHeight = 35;

          if (prescriptionMapYAxis > (PageHeight - prescriptionHeight)) {
            if (isOnPreviousPage) {
              pdf.setPage(pdf.internal.getNumberOfPages());
            } else {
              pdf.addPage();
            }
            prescriptionMapYAxis = 20;
          }

          pdf.addImage(pageData, 'PNG', (imgWidth / 2) + 15, prescriptionMapYAxis, (imgWidth / 2) - 15, prescriptionHeight);
          prescriptionMapYAxis = prescriptionMapYAxis + prescriptionHeight + 10;
        }
        yAxis = cardYAxis > prescriptionMapYAxis ? cardYAxis : prescriptionMapYAxis;
        pdf.setPage(pdf.internal.getNumberOfPages());
      }
      return yAxis;
    };
  const printPdf = async () => {
    const fileName = 'duly.pdf';
    try {
      const pdf = new jsPDF(undefined, 'pt', 'a4');
      const PageHeight = pdf.internal.pageSize.getHeight();
      const PageWidth = pdf.internal.pageSize.getWidth();
      let yAxis = 40;
      pdf.setFontSize(20);
      pdf.setTextColor('#002855');
      pdf.setFont('Helvetica', 'bold');
      pdf.text('Appointment Summary', 10, yAxis);
      pdf.setFontSize(8);
      pdf.text(moment().format('ddd, MMM D, YYYY - hh:m A'), 10, yAxis + 15);

      pdf.setFontSize(12);
      pdf.text('Patient-', PageWidth - 200, yAxis - 5);
      const givenNames: string[] = patientData?.generalInfo?.humanName?.givenNames || [];
      const familyName: string = patientData?.generalInfo?.humanName?.familyName || '';
      pdf.setTextColor('#04a6df');
      pdf.text(writeHumanName(givenNames, familyName), PageWidth - 150, yAxis - 5);
      pdf.setFontSize(6);
      pdf.setFont('Helvetica', 'normal');
      pdf.setTextColor('#002855');
      const gender: string = patientData?.gender || '';
      const birthDate: string | undefined = patientData?.birthDate;
      const yearsOld: number | undefined = calculateYears(birthDate);
      pdf.text(gender + '' + (yearsOld ? `, ${yearsOld} years old` : ''), PageWidth - 150, yAxis + 5);
      try {
        const photo = getSrcAvatar(patientData?.photo);
        if (photo)
          pdf.addImage(photo, 'PNG', PageWidth - 60, yAxis - 20, 30, 30);
        else
          pdf.addImage(nonEmployeePngIcon, 'png', PageWidth - 60, yAxis - 20, 30, 30);
      } catch (e) {
        console.log(e);
      }
      yAxis = yAxis + 40;
      const topDivs: any = document.querySelectorAll('[class*=\'topDiv\']')!;

      for (const topDiv of topDivs) {
        const header = topDiv.querySelector('.accordion-header');
        const hederHeight = 30;

        const canvas = await html2canvas(header, {
          useCORS: true,
          allowTaint: true,
        });

        const imgWidth = PageWidth - 20;
        const pageData = canvas.toDataURL('image/png', {
          scale: 3,
          optimized: false,
        });
        if (yAxis > (PageHeight - hederHeight)) {
          pdf.addPage();
          yAxis = 20;
        }

        pdf.addImage(pageData, 'PNG', 10, yAxis, imgWidth, hederHeight);
        yAxis = yAxis + hederHeight;


        topDiv.querySelector('.accordion-collapse')?.classList.add('show');
        const cards = topDiv.querySelectorAll('.appointment-card');


        yAxis = await creatAppoinmentCard(pdf, cards, yAxis, PageHeight, imgWidth, null);
        yAxis = await printPrescription(pdf, topDiv, yAxis, PageHeight, imgWidth);
      }
      pdf.save(fileName);
      setIsPdfExporting(false);
      setPdfExportStatusText('');
    } catch (ex) {
      setIsPdfExporting(false);
      setPdfExportStatusText('');
    }
  };

  const exportPdf = () => {
    setPdfExportStatusText('Generating PDF...');
    setIsPdfExporting(true);
    setAllAccordionOpen(defaultAccordionState);
    setTimeout(printPdf, 500);
  };

  const handleAllAccordion = (name: string) => {
    setAllAccordionOpen({
      ...allAccordionOpen,
      [name]: allAccordionOpen[name] == '0' ? '1' : '0',
    });
  };



  let referralCardDetails: CardDetailsType[] = [];

  const getPendingReferralDetails = () => {
    referralDetails && referralDetails.forEach((detail) => {
      let flag = 0;
      scheduledReferralDetails && scheduledReferralDetails.forEach((scheduledDetail) => {
        if (detail.providerType.toLowerCase() === scheduledDetail.refVisitType?.toLowerCase()) {
          if (scheduledDetail.skipped === false) {
            flag = 1;
          }
        }
      });
      if (!flag) {
        referralCardDetails.push({
          cardHeader: [
            { dateAndTime: `Pending` },
          ],
          cardColumns: [
            {
              topLine: 'REFERRAL VISIT',
              middleLine: detail.providerType,
            },
          ],
        });
      }
    });
  };

  const getReferralCardDetails = () => {
    referralCardDetails = [];
    scheduledReferralDetails && scheduledReferralDetails.forEach((referral) => {
      if (!referral.skipped) {
        let providerDetails: ProviderDetailsType | undefined = undefined;
        referralDetails && referralDetails.forEach((details) => {
          if (details.providerType.toLowerCase() === referral.refVisitType?.toLowerCase()) {
            providerDetails = details.providerDetails;
          }
        });
        providerDetails ?
          referralCardDetails.push({
            date: formatDateStringAddComma(new Date(referral.aptScheduleDate).toDateString()),
            cardHeader: [
              { dateAndTime: `${formatDateStringAddComma(new Date(referral.aptScheduleDate).toDateString())} ${referral.bookingSlot}` },
              {
                providerDetails: {
                  name: `Dr. ${providerDetails.humanName.familyName}`,
                  photo: providerDetails.photo.url,
                },
              },
            ],
            cardColumns: [
              {
                topLine: 'LOCATION',
                middleLine:
                  firstLetterCapital(providerDetails.location.address.addressLine),
                bottomLine: formatAddressBottomLine(
                  {
                    city: providerDetails.location.address.city,
                    postalCode: providerDetails.location.address.zipCode,
                    state: providerDetails.location.address.state,
                  }
                ),
              },
              {
                topLine: 'REFERRAL VISIT',
                middleLine: referral.refVisitType,
              },
              {
                topLine: 'DURATION',
                middleLine: '30 Minutes',
              },
            ],
          }) :
          referralCardDetails.push({
            date: formatDateStringAddComma(new Date(referral.aptScheduleDate).toDateString()),
            cardHeader: [
              { dateAndTime: `${formatDateStringAddComma(new Date(referral.aptScheduleDate).toDateString())} ${referral.bookingSlot}` },
              {
                providerDetails: {
                  name: '',
                  photo: '',
                },
              },
            ],
            cardColumns: [
              {
                topLine: 'LOCATION',
                middleLine: '',
                bottomLine: '',
              },
              {
                topLine: 'REFERRAL VISIT',
                middleLine: referral.refVisitType,
              },
              {
                topLine: 'DURATION',
                middleLine: '30 Minutes',
              },
            ],
          });
      }
    });
    getPendingReferralDetails();
  };

  let imagingCardDetails: CardDetailsType[] = [];

  const getPendingImagingDetails = () => {
    imagingTestDetails && imagingTestDetails.testOrder.map((testDetail) => {
      let flag = 0;
      scheduledImagingTestDetails && scheduledImagingTestDetails.forEach((scheduledDetail) => {
        if (testDetail.orderName.toLowerCase() === scheduledDetail.imagingType?.toLowerCase()) {
          if (scheduledDetail.skipped === false) {
            flag = 1;
          }
        }
      });
      if (!flag) {
        imagingCardDetails.push({
          cardHeader: [
            { dateAndTime: `Pending` },
          ],
          cardColumns: [
            {
              topLine: 'APPOINTMENT TYPE',
              middleLine: testDetail.orderName,
              bottomLine: 'Imaging Test',
            },
          ],
        });
      }
    });
  };

  const getImagingCardDetails = () => {
    imagingCardDetails = [];
    scheduledImagingTestDetails && scheduledImagingTestDetails.map((imagingDetails) => {
      if (!imagingDetails.skipped) {
        imagingCardDetails.push({
          cardHeader: [
            { dateAndTime: `${imagingDetails.aptScheduleDate && formatDateStringAddComma(new Date(imagingDetails.aptScheduleDate).toDateString())} - ${imagingDetails.bookingSlot}` },
          ],
          cardColumns: [
            {
              topLine: 'LOCATION',
              middleLine: imagingDetails.imagingLocation ? getFollowUpAppointmentLocation(
                imagingDetails.imagingLocation
              ).location : '',
              bottomLine: imagingDetails.imagingLocation ? getFollowUpAppointmentLocation(
                imagingDetails.imagingLocation
              ).pincode : '',
            },
            {
              topLine: 'APPOINTMENT TYPE',
              middleLine: imagingDetails.imagingType,
              bottomLine: 'Imaging Test',
            },
          ],
        });
      }
    });
    getPendingImagingDetails();
  };

  useEffect(() => {
    getReferralCardDetails();
    setReferralCardData(referralCardDetails);
  }, [ scheduledReferralDetails, referralDetails ]);

  useEffect(() => {
    getImagingCardDetails();
    setImagingCardData(imagingCardDetails);
  }, [ scheduledImagingTestDetails, imagingTestDetails ]);

  const prescriptionArray = prescriptionData ?
    [ ...prescriptionData.regular, ...prescriptionData.other ] : [];

  const followUpScheduled = () => {
    const cardData = [];
    if (isFollowUpScheduled && scheduledFollowUpDetails && followUpOrderDetails) {
      scheduledFollowUpDetails.forEach(followUp =>
        cardData.push({
          date: formatDateStringAddComma(new Date(followUp.aptScheduleDate).toDateString()),
          cardHeader: [
            { dateAndTime: `${formatDateStringAddComma(new Date(followUp.aptScheduleDate).toDateString())} ${followUp.bookingSlot}` },
            {
              providerDetails: {
                name: `Dr. ${followUpOrderDetails.practitioner.humanName.familyName}`,
                photo: followUpOrderDetails.practitioner.photo ? followUpOrderDetails.practitioner.photo.url : '',
              },
            },
          ],
          cardColumns: [
            {
              topLine: 'LOCATION',
              middleLine: followUpOrderDetails.location.address?.line,
              bottomLine: formatAddressBottomLine(
                {
                  city: followUpOrderDetails.location.address?.city,
                  postalCode: followUpOrderDetails.location.address?.postalCode,
                  state: followUpOrderDetails.location.address?.state,
                }
              ),
            },
            {
              topLine: 'APPOINTMENT TYPE',
              middleLine: followUp.aptType,
            },
            {
              topLine: 'DURATION',
              middleLine: '30 Minutes',
            },
          ],
        }));
    }
    if (!isFollowUpScheduled && followUpOrderDetails) {
      cardData.push({
        cardHeader: [
          { dateAndTime: `Pending` },
          {
            providerDetails: {
              name: `Dr. ${followUpOrderDetails.practitioner.humanName.familyName}`,
              photo: followUpOrderDetails.practitioner.photo ? followUpOrderDetails.practitioner.photo.url : '',
            },
          },
        ],
        cardColumns: [
          {
            topLine: 'LOCATION',
            middleLine: followUpOrderDetails.location.address?.line,
            bottomLine: formatAddressBottomLine(
              {
                city: followUpOrderDetails.location.address?.city,
                postalCode: followUpOrderDetails.location.address?.postalCode,
                state: followUpOrderDetails.location.address?.state,
              }
            ),
          },
        ],
      });
    }
    {
      if (followUpOrderDetails) {
        return (
          <AfterStepsAccordian
            header={followUpHeaderData}
            cardDetails={cardData}
          />
        );
      }
    }
  };

  const labTestScheduled = () => {
    {
      if (isLabTestScheduled 
          && 
          scheduledLabTestDetails 
          && labTestDetails 
          && labTestDetails.orderCount > 0) {
        return (
          <AfterStepsAccordian
            header={labsHeaderData}
            handleAccordion={handleAllAccordion}
            accordionOpen={allAccordionOpen}
            name="labs"
            cardDetails={labTestDetails?.testOrder?.map(test =>
            ({
              cardColumns: [
                {
                  topLine: 'LOCATION',
                  middleLine: scheduledLabTestDetails.lab_Name,
                  bottomLine: scheduledLabTestDetails.lab_Location,
                },
                {
                  topLine: 'APPOINTMENT TYPE',
                  middleLine: test.orderName,
                  bottomLine: 'Lab Test',
                },
              ],
            }))}
          />
        );
      }
      if(! isLabTestScheduled 
        && labTestDetails 
        && labTestDetails.orderCount > 0){
        return (
          <AfterStepsAccordian
            header={labsHeaderData}
            handleAccordion={handleAllAccordion}
            accordionOpen={allAccordionOpen}
            name="labs"
            cardDetails={labTestDetails.testOrder.map(test =>
            ({
              cardHeader: [
                { dateAndTime: `Pending` },
              ],
              cardColumns: [
                {
                  topLine: 'APPOINTMENT TYPE',
                  middleLine: test.orderName,
                  bottomLine: 'Lab Test',
                },
              ],
            }))}
          />
        );
      }
    }
  };
  return (
    <>
      <div className={styles.exportPdfIconContainer}>
        {isPdfExporting ? (
          <div className={styles.pdfExportStatus}>
            {pdfExportStatusText} <DulyLoader width={10} />
          </div>
        )
          : null
        }
        <div
          role="none"
          onKeyDown={noop}
          onClick={exportPdf}
          className={styles.exportPdfIcon}
        >
          <img src={ExportPdf} alt="exportpdficon" />
        </div>

        <img src={Line} alt="exportpdficon" className={styles.exportPdfIconLine} />

      </div>
      {(followUpOrderDetails === null
        && imagingTestDetails && imagingTestDetails.orderCount === 0
        && referralDetails && referralDetails.length === 0
        && labTestDetails && labTestDetails.orderCount === 0
        && prescriptionArray.length === 0
      ) ?
        <div className={styles.emptyContainer} /> : (
          <div className={styles.mainDiv}>
            {followUpScheduled()}
            {labTestScheduled()}
            {referralDetails
              && referralDetails.length > 0
              && (
                <AfterStepsAccordian
                  header={referralHeaderData}
                  cardDetails={referralCardData}
                  cardClass="refferal-card"
                />
              )
            }
            {imagingTestDetails
              && imagingTestDetails.orderCount > 0
              && (
                <AfterStepsAccordian
                  header={imagingHeaderData}
                  cardDetails={imagingCardData}
                />
              )
            }

            {
              !(prescriptionArray && !prescriptionArray.length) && (
                <PrescriptionAccordian
                  accordionOpen={allAccordionOpen}
                  handleAccordion={handleAllAccordion}
                  name="prescription"
                />
              )
            }
          </div >
        )}
    </>

  );
};
