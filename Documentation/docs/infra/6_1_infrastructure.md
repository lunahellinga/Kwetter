## Infrastucture

> Note: Currently, Kwetter can only be ran locally, as no support has been added for running it online

### General

After testing Kong and various other API gateways, we settled on using simple nginx ingresses to expose services on.
These, in combination with Metallb, allow us to access services inside the cluster(s) 

nginx
cert-manager
rbac-manager
cilium networking and hubble ui



### Automated deployment via ArgoCD 


### Monitoring

Prometheus
node-problem-detector

### Keycloak
