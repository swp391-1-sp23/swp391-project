import { useMutation, useQuery } from "@tanstack/react-query";
import { message } from "antd";
import { useAtom } from "jotai";
import { RESET } from "jotai/utils";
import { useTranslation } from "react-i18next";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";
import queryKeys from "../apis/keys/queryKeys";
import { profileAtom } from "./atoms";
import useTokenStore from "./useTokenStore";

const useProfileStore = (props?: {
  onUpdate?: (data?: components["schemas"]["AccountDto"]) => void;
}) => {
  const { token } = useTokenStore();
  const [profile, setProfile] = useAtom(profileAtom);
  const [mapi, ctx] = message.useMessage();
  const { t } = useTranslation();
  const resetProfile = () => setProfile(RESET);

  const query = useQuery({
    enabled: !!token,
    ...queryKeys.profileQueryKeys.get,
    onSuccess: ({ data: { data } }) => {
      mapi.success([t("QUERY.PROFILE.SUCCESS")].join(" "));
      if (!!data) setProfile(data);
      if (props?.onUpdate) props.onUpdate(data);
    },
    refetchOnMount: true,
  });

  const mutation = useMutation({
    mutationKey: ["put", "profile"],
    mutationFn: async (body: components["schemas"]["UpdateAccountDto"]) => {
      const putProfile = apiRequest.path("/api/profile").method("put").create();

      return await putProfile(body);
    },
    onMutate: (body) => {
      resetProfile();
      return body;
    },
    onError: (error) => {
      mapi.error([t("MUTAION.PROFILE.ERROR")].join(" "));
    },
    onSuccess: (data, body, ctx) => {
      mapi.success([t("MUTAION.PROFILE.SUCCESS")].join(" "));
    },
    onSettled: (data, error, body, ctx) => {
      query.refetch();
    },
  });

  return {
    profile,
    setProfile: mutation.mutate,
    resetProfile,
    mutation,
    ctx,
  };
};

export default useProfileStore;
