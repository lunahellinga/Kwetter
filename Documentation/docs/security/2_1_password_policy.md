## Username and Password Policy

The following policy is OWASP Top 10 Compliant

### Password Complexity:

- Users must create passwords that meet the following complexity requirements:
    - Minimum password length: 8 characters
    - Maximum password length: 64 characters
    - Any kind of character can be used, including unicode and whitespace
- Passwords should not contain easily guessable information such as names, birthdates, or commonly used words.

### Password Expiration and History:

- Passwords do not expire
- In case of a data breach, users are forced to change passwords on next login
- Users cannot reuse their previous 5 passwords.

### Account Lockout:

- After 5 unsuccessful login attempts, the user's account should be locked for a duration of 30 minutes.
- Administrators can manually unlock the account if necessary.

### Multi-Factor Authentication (MFA):

- Users should be encouraged to enable MFA for their accounts.
- MFA can be implemented using methods such as SMS verification, email verification, or authenticator apps.

### Secure Password Storage:

- Passwords must be securely hashed and salted using strong cryptographic algorithms (e.g., bcrypt or Argon2).
- Passwords should never be stored in plaintext or reversible encryption.

### Usernames:

- Usernames should not reveal any sensitive information.

### Account Recovery:

- Account recovery should require additional verification steps, such as sending a verification code to the user's
  registered email address or phone number.

### Secure Password Transmission:

- Passwords should only be transmitted over encrypted channels, such as HTTPS, to prevent interception and
  unauthorized access.

### Password Reset:

- When resetting passwords, a secure password reset process should be followed, including identity verification and
  temporary passwords that expire after a limited time or first use.