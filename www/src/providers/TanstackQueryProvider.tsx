import {
  QueryClient,
  QueryClientProvider,
  QueryClientProviderProps,
} from "@tanstack/react-query";

import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

export const queryClient = new QueryClient();

const TanstackQueryProvider = (props: QueryClientProviderProps) => {
  const { children, ...otherProps } = props;
  return (
    <QueryClientProvider {...otherProps}>
      {children}
      <ReactQueryDevtools />
    </QueryClientProvider>
  );
};

export default TanstackQueryProvider;
