interface CommonObject {
  [key: string | number]:
    | undefined
    | null
    | boolean
    | number
    | string
    | CommonObject
    | CommonObject[];
}

export default CommonObject;
