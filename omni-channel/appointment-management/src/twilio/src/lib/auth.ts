/* eslint-disable max-classes-per-file */
import jwt, { JwtPayload } from 'jsonwebtoken';
import { getAppConfig } from './appConfig';

const config = getAppConfig();

export type EventWithHeaders = {
    request: {
        headers: Record<string, string>,
        cookies: Record<string, string>
    }
}

export class AuthError extends Error {}

export class MissingAuthError extends AuthError {}

export function verifyJWT(token: string, secret: string, permission?: string): JwtPayload {
  const payload = jwt.verify(token, secret);
  if (typeof payload === 'string') {
    throw new AuthError('Wrong JWT token');
  }
  if (payload?.iss !== config.AUTH_REQUIRED_ISSUER) {
    throw new AuthError('Wrong Issuer of the JWT');
  }
  if (payload?.aud !== config.AUTH_REQUIRED_AUDIENCE) {
    throw new AuthError('Wrong Audience of the JWT');
  }

  if (permission) {
    const { perms } = payload;
    if (!payload.perms || perms !== permission) {
      throw new AuthError('Missing permission');
    }
  }

  return payload;
}

export function generateJWT(
  secret: string,
  claims: Record<string, any>,
  expiresIn: string = '1h',
): string {
  return jwt.sign(
    { iss: config.AUTH_REQUIRED_ISSUER, aud: config.AUTH_REQUIRED_AUDIENCE, ...claims },
    secret,
    { expiresIn },
  );
}

export function validateEventForJWT(
  event: EventWithHeaders,
  secret: string,
  permission?: string,
): JwtPayload {
  const authHeader = event?.request?.headers?.authorization;
  if (!authHeader) {
    throw new MissingAuthError('Missing authorization');
  }

  const [authType, authToken] = authHeader.split(' ');
  if (authType.toLowerCase() !== 'bearer') {
    throw new MissingAuthError('Missing bearer authorization');
  }

  return verifyJWT(authToken, secret, permission);
}
