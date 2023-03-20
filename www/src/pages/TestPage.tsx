import { useTranslation } from "react-i18next";

const TestPage = () => {
  const { t } = useTranslation();
  return <>TestPage {t("HELLO")}</>;
};

export default TestPage;
