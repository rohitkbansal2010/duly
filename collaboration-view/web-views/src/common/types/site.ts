export type Address = {
  city?: string;
  line?: string;
  postalCode?: string;
  state?: string;
}

export type Site = {
  id: string;
  address?: Address;
}
