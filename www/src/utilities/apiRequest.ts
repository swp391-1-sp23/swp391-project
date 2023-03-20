import { Fetcher, Middleware } from "openapi-typescript-fetch";
import ENV from "src/constants/env";
import { paths } from "./apiSchemas";

const apiRequest = Fetcher.for<paths>();

const authMiddleware: Middleware = async (url, init, next) => {
  let token = localStorage.getItem("token");

  if (token) {
    token = token.replaceAll('"', "");

    const authHeaderExisted = init.headers.get("Authorization");

    switch (authHeaderExisted) {
      case null:
        init.headers.append("Authorization", ["Bearer", token].join(" "));
        break;
      default:
        init.headers.set("Authorization", ["Bearer", token].join(" "));
    }

    init.credentials = "include";
  }

  const response = await next(url, init);

  return response;
};

apiRequest.configure({
  baseUrl: ENV.API_BASE_URL,
  use: [authMiddleware],
});

export default apiRequest;
