import Authenticator from './authenticator';
import { generateJWT } from './auth';

export class JWTAuthenticator implements Authenticator {
  secret: string;

  claims: Record<string, any>;

  constructor(secret: string, claims: Record<string, any>) {
    this.secret = secret;
    this.claims = claims;
  }

  async acquireToken() {
    return generateJWT(this.secret, this.claims);
  }
}
