// import RCFooter from "rc-footer";
// import "rc-footer/assets/index.css";
import { Col, Layout, Row, Space, theme, Typography } from "antd";
import { CSSProperties } from "react";
import { useTranslation } from "react-i18next";

const { Footer: AntFooter } = Layout;
const { Title } = Typography;

const footerStyle: CSSProperties = {
  // textAlign: "center",
  // color: "#fff",
  // backgroundColor: "#7dbcea",
};

const Footer = () => {
  const { t } = useTranslation();
  const {
    token: { colorBgContainer },
  } = theme.useToken();
  return (
    <AntFooter
      style={{
        backgroundColor: colorBgContainer,
      }}
    >
      <Row justify={"space-around"}>
        {/* CONTACT */}
        <Col>
          <Space direction={"vertical"}>
            <Row>
              <Title>{t("FOOTER.CONTACT")}</Title>
            </Row>
            <Row>{t("FOOTER.CONTACT.ADDRESS")}</Row>
            <Row>{t("FOOTER.CONTACT.HOTLINE")}</Row>
            <Row>{t("FOOTER.CONTACT.EMAIL")}</Row>
          </Space>
        </Col>
        {/* SOCIAL */}
        <Col>
          <Space direction={"vertical"}>
            <Row>
              <Title>{t("FOOTER.SOCIAL")}</Title>
            </Row>
          </Space>
        </Col>
      </Row>
    </AntFooter>
  );
};

export default Footer;
