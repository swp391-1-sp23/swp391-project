import { Provider } from "jotai";
import { BaseProviderProps } from "./BaseProviderProps";

const StateProvider = (props: BaseProviderProps) => {
  const { children } = props;
  return <Provider>{children}</Provider>;
};

export default StateProvider;
