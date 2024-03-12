# Users
old_user = person "Usero "
# old_moderator = person "Moderator"
# old_admin = person "Admin"

# Systems
# old_cloudinary = softwareSystem "Cloudinary Image Hosting" "Hosts user profile pictures with static URLs" "Existing System"

old_brokers = softwareSystem "Brokers" {
    old_gateway_broker = container "Gateway Message Broker" "Broker for queuing low-priority user POST requests" "RabbitMQ" "Broker"
}

old_access_manager = softwareSystem "Access System" {
    old_dashboard = container "User WebApp" "Primary webapp that lets users kweet and see other's kweets." "${WEB}" "Web Browser"
    old_gateway = container "User API Gateway" "nginx API Gateway as the ingress for user requests" "nginx API Gateway"

    # Auth Service
    old_keycloak_db = container "Keycloak DB" "Keycloak data" "${SQL}" "Database"
    old_keycloak = container "Keycloak" "Manages and provides user authentication" "Keycloak" {
        -> old_keycloak_db
    }
}

# Kweet posting
old_post_manager = softwareSystem "Posting System" {
    old_kweeter_broker = container "Kweeter Message Broker" "Broker for parts of a kweet post" "RabbitMQ" "Broker"

    old_kweeter = container "Kweeter" "Main entrypoint for posting kweets, serves as an orchestrator for kweet creation"
    old_meta_db = container "Metadata DB" "Kweet Metadata" "${TBD}" "Database"
    old_meta_service = container "Metadata Service" "Tracks kweet metadata such as number of likes, shares and associated tags" {
        -> old_meta_db
    }
    old_kweeter_data_db = container "Kweeter Data DB" "Kweets" "${TBD}" "Database"
    old_kweeter_data_service = container "Kweeter Data Service" "Stores and serves kweets" {
        -> old_kweeter_data_db
    }
    old_tag_db = container "Tag DB" "Tags and linked kweets" "${TBD}" "Database"
    old_tag_service = container "Tag Service" "Tracks kweet tags" {
        -> old_tag_db
    }
    old_content_db = container "Content Store" "Kweet content storage" "${TBD}" "NoSQL"
    old_content_service = container "Content Service" "Stores and serves additional kweet content such as images" {
        -> old_content_db
    }
    old_mention_db = container "Mention DB" "Stores mentions" "${TBD}" "Database"
    old_mention_service = container "Mention Service" "Manages mentions links between kweets and users" {
        -> old_mention_db
    }
}

# Trends
old_trend_manager = softwareSystem "Trend System" {
    old_trend_data_db = container "Trend Data DB" "Stores tag and like based trend analysis data" "${TIME}" "Database"
    old_trend_cache_db = container "Trend Cache" "Stores trend analysis results" "${TBD}" "Database"
    old_trend_data_service = container "Trend Data Service" "Maintains tag and like data over time" {
        -> old_trend_data_db "Write"
    }
    old_trend_analysis_service = container "Trend Analyzer" "Makes use of trend data to maintain current trending topic list" {
        -> old_trend_data_db "Read raw trend data"
        -> old_trend_cache_db "Write trend analysis results"
    }
    old_trend_service = container "Trend Service" "Provides results of trend analysis for usage" {
        -> old_trend_cache_db "Read trend analysis results"
    }
}
# Replying
old_interaction_manager = softwareSystem "Interaction System" {
    old_reply_db = container "Reply DB" "Stores reply links" "${TBD}" "Database"
    old_reply_service = container "Reply Service" "Manages links between kweets" {
        -> old_reply_db
    }
    old_like_db = container "Like DB" "Stores likes" "${TBD}" "Database"
    old_like_service = container "Like Service" "Manages user like history" {
        -> old_like_db
    }
    old_share_db = container "Share DB" "Stores shares" "${TBD}" "Database"
    old_share_service = container "Share Service" "Manages user share history" {
        -> old_share_db
    }
    old_follow_db = container "Follow DB" "Who users follow" "${SQL}" "Database"
    old_follow_service = container "Follow Service" "Manages user follows" {
        -> old_follow_db
    }
}

# Reading kweets
old_kweet_reader = softwareSystem "Reading System" {
    old_kweader_cache = container "Kweader Cache" "Recent and high-traffic kweads" "${NOSQL}" "NoSQL"
    old_kweader_cache_service = container "Kweader Cache Service" "Data service for fast access to high traffic and recent kweets" {
        -> old_kweader_cache
    }
    old_kweader = container "Kweader" "Main entrypoint for fetching kweet data"
    old_timeline_cache = container "Timeline Cache" "Stores trend timelines" "${NOSQL}" "NoSQL"
    old_timeline_builder = container "Timeline Builder" "Generates timelines for users and trends" {
        -> old_timeline_cache
    }
    old_search_engine = container "Search Engine" "Searches users or kweets based on given parameters"
}


# Top level
user -> old_dashboard "Uses"
# dashboard -> cloudinary "Fetch images"
# profiler -> cloudinary "Upload user images"

# Gateway usage
old_dashboard -> old_gateway "Sends requests" "REST" "tag_gateway"
old_gateway -> old_gateway_broker "Queue low priority POSTs" "AMQP 0-9-1" "tag_gateway"
old_gateway -> old_keycloak "Sends auth requests" "REST" "tag_gateway"
old_gateway -> old_follow_service "Get list of followed/followers" "REST" "tag_gateway"
old_gateway_broker -> old_follow_service "Queue follow related requests" "AMQP 0-9-1" "tag_gateway"
old_gateway_broker -> old_like_service "Queue like addition and removal from kweet" "AMQP 0-9-1" "tag_gateway"
old_gateway_broker -> old_share_service "Queue sharing or unsharing a kweet" "AMQP 0-9-1" "tag_gateway"
old_gateway_broker -> old_kweeter "Queue posting kweets and replies" "AMQP 0-9-1"
old_gateway -> old_timeline_builder "Request timeline for: own, user, tag, shared_by, liked_by" "REST" "tag_gateway"
old_gateway -> old_trend_data_service "Get current trends" "REST" "tag_gateway"
old_gateway -> old_kweader "Requests kweet data for reading" "REST" "tag_gateway"

# Post a kweet or reply
old_kweeter -> old_kweeter_broker "Queue kweet posting jobs, consume failed_jobs" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_kweeter_data_service "Consume store_kweet" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_kweader_cache_service "Consume cache_update" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_meta_service "Consume register_kweet_metadata" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_tag_service "Consume register_tags" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_content_service "Consume store_kweet_content" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_mention_service "Consume register_mentions" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_reply_service "Consume register_reply" "AMQP 0-9-1" "tag_kweeter"
old_kweeter_broker -> old_trend_data_service "Consume register_tags" "AMQP 0-9-1" "tag_kweeter"

# Read content of a kweet
old_kweader -> old_meta_service "Get kweet metadata" "" "tag_kweader"
old_kweader -> old_kweader_cache_service "Get cached kweets" "" "tag_kweader"
old_kweader -> old_kweeter_data_service "Get not-cached kweets" "" "tag_kweader"
old_kweader -> old_content_service "Get kweet content" "" "tag_kweader"

# Get timeline for something
old_timeline_builder -> old_follow_service "Get followed users" "" "tag_timeline"
old_timeline_builder -> old_mention_service "Get mentioned kweets" "" "tag_timeline"
old_timeline_builder -> old_share_service "Get kweets shared by" "" "tag_timeline"
old_timeline_builder -> old_tag_service "Get tag information" "" "tag_timeline"
old_timeline_builder -> old_like_service "Get liked kweets" "" "tag_timeline"
old_timeline_builder -> old_meta_service "Get kweets for user, get metadata" "" "tag_timeline"
old_timeline_builder -> old_reply_service "Get reply context of kweets" "" "tag_timeline"
old_timeline_builder -> old_search_engine "Forward search parameters" "" "tag_timeline"

# Search for stuff
old_search_engine -> old_keycloak "Search for users" "" "tag_search"
old_search_engine -> old_meta_service "Search based on metadata" "" "tag_search"

