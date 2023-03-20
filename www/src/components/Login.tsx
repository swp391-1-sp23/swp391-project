import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Button, Form, Input, message, Modal, Space } from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import queryKeys from "src/hooks/apis/keys/queryKeys";
import useTokenStore from "src/hooks/states/useTokenStore";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const { useMessage } = message;
const { useForm, Item: FormItem, List: FormList } = Form;

const Login = () => {
  const [form] = useForm();
  const { t } = useTranslation();
  const [mapi, ctx] = useMessage();
  const queryClient = useQueryClient();
  const { setToken } = useTokenStore();
  const [ismodalOpen, setIsModalOpen] = useState(false);
  const toggleModal = () => setIsModalOpen((state) => !state);
  const onCancel = () => {
    setIsModalOpen(false);
  };

  const mutation = useMutation({
    mutationKey: ["post", "login"],
    mutationFn: async (body: components["schemas"]["LoginInput"]) => {
      const postLogin = apiRequest.path("/api/login").method("post").create();

      const response = await postLogin({ ...body });

      return response;
    },
    onError: (error, body) => {
      mapi.error([t("FORM.LOGIN.ON_ERROR")].join(" "));
    },
    onSuccess: ({ data: { data } }, body, ctx) => {
      mapi.success([t("FORM.LOGIN.ON_SUCCESS")].join(" "));
      setToken(data as string);
      queryClient.invalidateQueries({
        queryKey: queryKeys.profileQueryKeys._def,
      });
    },
    onSettled: (data, error, body, ctx) => {
      if (!error) setIsModalOpen(false);
    },
  });

  const onFinish = (values: components["schemas"]["LoginInput"]) => {
    mutation.mutate(values);
  };

  const onReset = () => {
    form.resetFields();
  };

  return (
    <>
      <Button type={"text"} onClick={toggleModal}>
        {t("FORM.LOGIN")}
        {ctx}
      </Button>
      <Modal
        title={t("FORM.LOGIN")}
        open={ismodalOpen}
        onCancel={onCancel}
        confirmLoading={mutation.isLoading}
        footer={null}
      >
        <Form
          form={form}
          onFinish={onFinish}
          layout={"vertical"}
          initialValues={{
            email: "user@example.com",
            password: "string",
          }}
        >
          <FormItem
            name={"email"}
            label={t("FORM.LOGIN.EMAIL")}
            required={true}
            rules={[
              {
                type: "email",
                required: true,
                message: t("FORM.LOGIN.EMAIL.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input />
          </FormItem>
          <FormItem
            name={"password"}
            label={t("FORM.LOGIN.PASSWORD")}
            required={true}
            rules={[
              {
                type: "string",
                min: 6,
                required: true,
                message: t("FORM.LOGIN.PASSWORD.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input.Password />
          </FormItem>
          <FormItem>
            <Space>
              <Button onClick={onReset} disabled={mutation.isLoading}>
                {t("FORM.LOGIN.RESET")}
              </Button>
              <Button
                htmlType={"submit"}
                type={"primary"}
                loading={mutation.isLoading}
              >
                {t("FORM.LOGIN.SUBMIT")}
              </Button>
            </Space>
          </FormItem>
        </Form>
      </Modal>
    </>
  );
};

export default Login;
