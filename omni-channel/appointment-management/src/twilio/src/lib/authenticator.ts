export default interface Authenticator {
    acquireToken(): Promise<string>;
}
