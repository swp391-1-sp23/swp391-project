import {
  createQueryKeys,
  inferQueryKeyStore,
  mergeQueryKeys,
} from "@lukemorales/query-key-factory";
import apiRequest from "src/utilities/apiRequest";

const addressQueryKeys = createQueryKeys("addressQueryKeys", {
  get: {
    queryKey: ["address"],
    queryFn: async () => {
      const getAddress = apiRequest.path("/api/address").method("get").create();

      return await getAddress({});
    },
  },
});

const cartQueryKeys = createQueryKeys("cartQueryKeys", {
  get: {
    queryKey: ["cart"],
    queryFn: async () => {
      const getCart = apiRequest.path("/api/cart").method("get").create();

      return await getCart({});
    },
  },
});

const orderQueryKeys = createQueryKeys("orderQueryKeys", {
  get: {
    queryKey: ["order"],
    queryFn: async () => {
      const getOrder = apiRequest.path("/api/order").method("get").create();

      return await getOrder({});
    },
  },
  getByTag: ({ orderTag }) => ({
    queryKey: ["order", orderTag],
    queryFn: async () => {
      const getOrderByTag = apiRequest
        .path("/api/order/{orderTag}")
        .method("get")
        .create();

      return await getOrderByTag({ orderTag });
    },
  }),
});

const profileQueryKeys = createQueryKeys("profileQueryKeys", {
  get: {
    queryKey: ["profile"],
    queryFn: async () => {
      const getProfile = apiRequest.path("/api/profile").method("get").create();

      return await getProfile({});
    },
  },
});

const accountQueryKeys = createQueryKeys("accountQueryKeys", {
  get: (params: { SearchKey?: string }) => ({
    queryKey: ["account", params],
    queryFn: async () => {
      const getAccount = apiRequest.path("/api/account").method("get").create();

      return await getAccount(params);
    },
  }),
});

const authQueryKeys = createQueryKeys("authQueryKeys", {
  checkToken: {
    queryKey: ["checkToken"],
    queryFn: async () => {
      const getCheckToken = apiRequest
        .path("/api/checkToken")
        .method("get")
        .create();

      return await getCheckToken({});
    },
  },
});

const brandQueryKeys = createQueryKeys("brandQueryKeys", {
  get: ({ params }) => ({
    queryKey: ["brand", params],
    queryFn: async () => {
      const getBrand = apiRequest.path("/api/brand").method("get").create();

      return await getBrand({});
    },
  }),
});

const productQueryKeys = createQueryKeys("productQueryKeys", {
  get: ({ params }) => ({
    queryKey: ["product", params],
    queryFn: async () => {
      const getProduct = apiRequest.path("/api/product").method("get").create();

      return await getProduct({});
    },
  }),
  getById: ({ productId }: { productId: string }) => ({
    queryKey: ["product", productId],
    queryFn: async () => {
      const getProductById = apiRequest
        .path("/api/product/{productId}")
        .method("get")
        .create();

      return await getProductById({ productId });
    },
  }),
});

const queryKeys = mergeQueryKeys(
  addressQueryKeys,
  cartQueryKeys,
  orderQueryKeys,
  profileQueryKeys,
  accountQueryKeys,
  authQueryKeys,
  brandQueryKeys,
  productQueryKeys
);

export type QueryKeys = inferQueryKeyStore<typeof queryKeys>;

export default queryKeys;
