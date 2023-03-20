import { useSize } from "ahooks";
import { Layout } from "antd";
import Content from "src/components/Content";
import Footer from "src/components/Footer";
import Header from "src/components/Header";

const RootPage = () => {
  const size = useSize(document.querySelector("body"));
  return (
    <Layout
      style={{
        height: size?.height! < size?.width! ? "100vmin" : "100vmax",
      }}
    >
      <Header />
      <Layout>
        <Content />
      </Layout>
      <Footer />
    </Layout>
  );
};

export default RootPage;
