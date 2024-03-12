## Configuration Management

Configurations are managed at three points for different configuration types:

### Infrastructure Configuration:

- Infrastructure configuration is managed in
  the [Infrastructure](https://dev.azure.com/OIBSS-F/Kwetter/_git/Infrastructure) repository.
- This includes configuration related to the infrastructure setup, such as networking, virtual machines, and cloud
  services.

### Application Configuration:

- First-party application-specific configuration, such as service configuration, is managed in
  the [Helm Charts](https://github.com/davidhellinga/kwetter-web-helm) repository.
- This includes configuration files specific to the Kwetter application, such as API endpoints, database
  connections, and feature toggles.

### Environmental Variables and Sensitive Configuration:

- Environmental variables and sensitive configuration, such as secrets or keys, are managed in the DevOps
  environment [library](https://dev.azure.com/OIBSS-F/Kwetter/_library).
- Environmental variables provide runtime configuration options, while sensitive information is stored securely and
  accessed through appropriate mechanisms.
- Secrets and keys must be stored as secret variables within the DevOps environment.
- Secret variables ensure that sensitive information remains encrypted and is accessible only to authorized users or
  services.

By following these guidelines, Kwetter ensures that configurations are properly managed and organized at the appropriate
points, allowing for efficient and secure configuration management throughout the project.