import { atomWithStorage } from "jotai/utils";
import { components } from "src/utilities/apiSchemas";

export const tokenAtom = atomWithStorage<string | null>("token", null);

export const profileAtom = atomWithStorage<
  components["schemas"]["AccountDto"] | null
>("profile", null);
