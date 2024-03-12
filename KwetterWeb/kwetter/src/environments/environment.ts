const BASE_URI = 'https://api.oibss.nl/'
export const environment = {
  production: true,
  keycloak: {
    url: 'https://keycloak.oibss.nl/',
    realm: 'Kwetter',
    clientId: 'kwetter-client'
  },
  kwetter: {
    gateway: {
      recent: BASE_URI + 'recent',
      post: BASE_URI + 'post-kweet',
      gdpr: BASE_URI + 'gdpr-delete',
    },
    conf: {
      kweet_max_length: 140
    }

  }
};
