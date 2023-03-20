import { useAtom } from "jotai";
import { RESET } from "jotai/utils";
import { tokenAtom } from "./atoms";

const useTokenStore = () => {
  const [token, setToken] = useAtom(tokenAtom);

  const resetToken = () => setToken(RESET);

  return { token, setToken, resetToken };
};

export default useTokenStore;
