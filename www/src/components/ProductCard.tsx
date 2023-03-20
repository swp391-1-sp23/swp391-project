import { Badge, Card, CardProps, Col } from "antd";
import { useTranslation } from "react-i18next";

const ProductCard = (props: {
  cardProps?: CardProps;
  productProps: {
    productName: string;
    price: number;
    brand: string;
  };
}) => {
  const { t } = useTranslation();
  return (
    <Col xs={12} sm={8} md={6}>
      <Badge.Ribbon
        text={t("PRODUCT_CARD.PRICE", { price: props.productProps.price })}
      >
        <Card hoverable={true} {...props?.cardProps}>
          <Card.Meta
            title={props.productProps.productName}
            description={props.productProps.brand}
          />
        </Card>
      </Badge.Ribbon>
    </Col>
  );
};

export default ProductCard;
