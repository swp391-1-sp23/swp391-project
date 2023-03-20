import { ConfigProvider } from "antd";
import { ConfigProviderProps } from "antd/es/config-provider";

interface Props extends ConfigProviderProps {}

const AntConfigProvider = (props: Props) => {
  const { children, ...otherProps } = props;
  return <ConfigProvider children={children} {...otherProps} />;
};

export default AntConfigProvider;
