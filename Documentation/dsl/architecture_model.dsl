# Users
user = person "User"
# moderator = person "Moderator"
# admin = person "Admin"

# Systems
# cloudinary = softwareSystem "Cloudinary Image Hosting" "Hosts user profile pictures with static URLs" "Existing System"

kwetter_system = softwareSystem "Kwetter" {

    dashboard = container "Kwetter WebApp" "Primary webapp that lets users kweet and see other's kweets." "${WEB}" "Web Browser"
    gateway = container "API Gateway" "GCE API Gateway as the ingress for user requests" "GCE API Gateway" "Existing System"
    kwetter_api = container "Kwetter Gateway API" "Gateway API for authenticating and routing user requests" "${NET}"
    broker = container "Message Broker" "Message bus for async messaging" "RabbitMQ" "Broker|Existing System"

    # Auth Service
    keycloak_db = container "Keycloak DB" "Realm settings, authentication policies, user data" "${SQL}" "Database"
    keycloak = container "Keycloak" "Manages and provides user authentication and authorization" "Keycloak (Bitnami)" "Existing System"{
        -> keycloak_db
    }


# Kweet posting
    kweeter_store = container "Kweeter Saga Store" "${NOSQL}" "NoSQL"
    kweeter = container "Kweeter" "Main entrypoint for posting kweets, serves as an orchestrator for kweet creation"{
        -> kweeter_store
    }

    meta_db = container "Metadata DB" "Kweet Metadata" "${DISTSQL}" "Database"
    meta_service = container "Metadata Service" "Tracks kweet metadata such as number of likes, shares and associated tags" {
        -> meta_db
    }
    kweeter_data_db = container "Kweeter Data DB" "Kweets" "${DISTSQL}" "Database"
    kweeter_data_service = container "Kweeter Data Service" "Stores and serves kweets" {
        -> kweeter_data_db
    }
    tag_db = container "Tag DB" "Tags and linked kweets" "${DISTSQL}" "Database"
    tag_service = container "Tag Service" "Tracks kweet tags" {
        -> tag_db
    }
    content_db = container "Content Store" "Kweet content storage" "${DISTSQL}" "NoSQL"
    content_service = container "Content Service" "Stores and serves additional kweet content such as images" {
        -> content_db
    }
    mention_db = container "Mention DB" "Stores mentions" "${DISTSQL}" "Database"
    mention_service = container "Mention Service" "Manages mentions links between kweets and users" {
        -> mention_db
    }


# Reading kweets
    kweader_mongo = container "Kweader MongoDB Storage" "Recent and high-traffic kwead storage" "${NOSQL}" "NoSQL"
    kweader_redis = container "Kweader Redis Cache" "Recent and high-traffic kwead caching" "${NOSQL}" "NoSQL"
    kweader_cache_service = container "Kweader Cache Service" "Data service for fast access to high traffic and recent kweets" {
        -> kweader_mongo
        -> kweader_redis
    }


# Top level
user -> dashboard "Uses"

# Gateway usage
dashboard -> gateway "Sends requests" "REST" "tag_gateway"
gateway -> kwetter_api "Route user kweet requests" "${MSG_MT}" "tag_gateway"
gateway -> keycloak "Route user auth requests" "REST" "tag_gateway"
kwetter_api -> keycloak "Verifies user token" "REST" "tag_gateway"
kwetter_api -> broker "Queue low priority POSTs" "${MSG_MT}" "tag_gateway"
kwetter_api -> kweader_cache_service "Requests kweet data for reading" "REST" "tag_gateway"
broker -> kweeter "Queue posting kweets and replies" "${MSG_MT}"

# Post a kweet or reply
kweeter -> broker "Queue kweet posting jobs, consume failed_jobs" "${MSG_MT}" "tag_kweeter"
broker -> kweeter_data_service "Consume TextCommand, publish TextCompleted" "${MSG_MT}" "tag_kweeter"
broker -> kweader_cache_service "Consume  CacheCommand, publish CacheCompleted" "${MSG_MT}" "tag_kweeter"
broker -> meta_service "Consume MetadataCommand, publish MetadataCompleted" "${MSG_MT}" "tag_kweeter"
broker -> tag_service "Consume TagCommand, publish TagCompleted" "${MSG_MT}" "tag_kweeter"
broker -> content_service "Consume ContentCommand, publish ContentCompleted" "${MSG_MT}" "tag_kweeter"
broker -> mention_service "Consume MentionCommand, publish MentionCompleted" "${MSG_MT}" "tag_kweeter"
}