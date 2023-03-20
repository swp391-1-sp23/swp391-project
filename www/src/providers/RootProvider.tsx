import AntConfigProvider from "./AntConfigProvider";
import { BaseProviderProps } from "./BaseProviderProps";
import StateProvider from "./StateProvider";
import TanstackQueryProvider, { queryClient } from "./TanstackQueryProvider";

const RootProvider = (props: BaseProviderProps) => {
  const { children } = props;
  return (
    <StateProvider>
      <TanstackQueryProvider client={queryClient}>
        <AntConfigProvider>{children}</AntConfigProvider>
      </TanstackQueryProvider>
    </StateProvider>
  );
};

export default RootProvider;
