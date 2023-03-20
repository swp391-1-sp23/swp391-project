import { useMutation } from "@tanstack/react-query";
import { Button, Form, Input, message, Modal, Space } from "antd";
import { useState } from "react";
import { useTranslation } from "react-i18next";
import apiRequest from "src/utilities/apiRequest";
import { components } from "src/utilities/apiSchemas";

const { useMessage } = message;
const { useForm, Item: FormItem, List: FormList } = Form;

const Register = () => {
  const { t } = useTranslation();
  const [ismodalOpen, setIsModalOpen] = useState(false);
  const toggleModal = () => setIsModalOpen((state) => !state);
  const onCancel = () => {
    toggleModal();
  };
  const onOk = () => {
    toggleModal();
  };

  const [mapi, ctx] = useMessage();

  const mutation = useMutation({
    mutationKey: [],
    mutationFn: async (body: components["schemas"]["RegisterInput"]) => {
      const postRegister = apiRequest
        .path("/api/register/{role}")
        .method("post")
        .create();

      return await postRegister({
        role: "Customer",
        ...body,
      });
    },
    onError: (error, body) => {
      mapi.error([t("FORM.REGISTER.ON_ERROR")].join(" "));
    },
    onMutate: (body) => body,
    onSuccess: (data, body, ctx) => {
      mapi.success([t("FORM.REGISTER.ON_SUCCESS")].join(" "));
    },
    onSettled: (data, error, body, ctx) => {
      if (!error) toggleModal();
    },
  });

  const [form] = useForm();

  const onFinish = (values: components["schemas"]["RegisterInput"]) => {
    // console.log(values);
    mutation.mutate(values);
  };

  const onReset = () => {
    form.resetFields();
  };

  return (
    <>
      <Button type={"text"} onClick={toggleModal}>
        {t("FORM.REGISTER")}
      </Button>
      <Modal
        title={t("FORM.REGISTER")}
        open={ismodalOpen}
        onCancel={onCancel}
        confirmLoading={mutation.isLoading}
        onOk={onOk}
        footer={null}
      >
        <Form form={form} onFinish={onFinish} layout={"vertical"}>
          <FormItem
            name={"firstName"}
            label={t("FORM.REGISTER.FIRST_NAME")}
            required={true}
            rules={[
              {
                type: "string",
                min: 2,
                required: true,
                message: t("FORM.REGISTER.FIRST_NAME.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input disabled={mutation.isLoading} />
          </FormItem>
          <FormItem
            name={"lastName"}
            label={t("FORM.REGISTER.LAST_NAME")}
            required={true}
            rules={[
              {
                type: "string",
                min: 2,
                required: true,
                message: t("FORM.REGISTER.LAST_NAME.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input disabled={mutation.isLoading} />
          </FormItem>
          <FormItem
            name={"phone"}
            label={t("FORM.REGISTER.PHONE")}
            required={true}
            rules={[
              {
                type: "regexp",
                pattern: /(\+84|0)\d{9}/,
                required: true,
                message: t("FORM.REGISTER.PHONE.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input disabled={mutation.isLoading} />
          </FormItem>
          <FormItem
            name={"email"}
            label={t("FORM.REGISTER.EMAIL")}
            required={true}
            rules={[
              {
                type: "email",
                required: true,
                message: t("FORM.REGISTER.EMAIL.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input disabled={mutation.isLoading} />
          </FormItem>
          <FormItem
            name={"password"}
            label={t("FORM.REGISTER.PASSWORD")}
            required={true}
            rules={[
              {
                type: "string",
                min: 6,
                required: true,
                message: t("FORM.REGISTER.PASSWORD.VALIDATE_MESSAGE")!,
              },
            ]}
          >
            <Input.Password disabled={mutation.isLoading} />
          </FormItem>
          <FormItem
            name={"confirmPassword"}
            label={t("FORM.REGISTER.CONFIRM_PASSWORD")}
            required={true}
            rules={[
              {
                type: "string",
                required: true,
                message: t("FORM.REGISTER.CONFIRM_PASSWORD.VALIDATE_MESSAGE")!,
              },
              ({ getFieldValue }) => ({
                validator: (rule, value, callback) => {
                  if (!value || value === getFieldValue("password"))
                    return Promise.resolve();
                  return Promise.reject(
                    new Error(
                      t("FORM.REGISTER.CONFIRM_PASSWORD.VALIDATE_MESSAGE")!
                    )
                  );
                },
              }),
            ]}
          >
            <Input.Password disabled={mutation.isLoading} />
          </FormItem>
          <FormItem>
            <Space>
              <Button onClick={onReset} disabled={mutation.isLoading}>
                {t("FORM.REGISTER.RESET")}
              </Button>
              <Button
                htmlType={"submit"}
                type={"primary"}
                loading={mutation.isLoading}
              >
                {t("FORM.REGISTER.SUBMIT")}
              </Button>
            </Space>
          </FormItem>
        </Form>
      </Modal>
      {ctx}
    </>
  );
};

export default Register;
