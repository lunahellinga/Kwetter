SystemLandscape "SystemLandscape" {
    include *
    # autoLayout
}

# systemContext access_manager "AccessSystemContext" {
#     include *
#     # autoLayout
# }

# systemContext post_manager "PostingSystemContext" {
#     include *
#     # autoLayout
# }

# systemContext trend_manager "TrendSystemContext" {
#     include *
#     # autoLayout
# }

# systemContext interaction_manager "InteractionSystemContext" {
#     include *
#     # autoLayout
# }

# systemContext kweet_reader "ReaderSystemContext" {
#     include *
#     # autoLayout
# }

container kwetter_system "Kwetter" {
    include *
    # autoLayout
}

dynamic kwetter_system "kweeter_saga"{
    title "Kweeter Saga"
    user -> dashboard "User posts new kweet"
    dashboard -> gateway "Kweet is sent to the API"
    gateway -> kwetter_api "Kweet is sent to the API"
    kwetter_api -> broker "New kweet is queued"
    broker -> kweeter "Saga is initialized, id is generated"
    # text
    kweeter -> broker "Text processing starts"
    broker -> kweeter_data_service "Text is sanitized and stored, injection attempts are logged"
    kweeter_data_service -> broker "Sanitized text is returned"
    broker -> kweeter "Sanitized text added to saga"
    # split
    kweeter -> broker "Split processing of tags, mentions and content starts"
    {
        {
            broker -> tag_service "Tags are found, registered and stored"
            tag_service -> broker "Tags and count are returned"
            broker -> kweeter "Tags and count added to saga"
        }
        {
            broker -> mention_service "Mentions are found, registered and stored"
            mention_service -> broker "Mentions and count are returned"
            broker -> kweeter "Mentions and count added to saga"
        }
        {
            broker -> content_service "Content is found, registered and stored"
        }
    }
    
    content_service -> broker "Content links and count are returned"
    broker -> kweeter "Content links and count added to saga"
    # meta
    kweeter -> broker "Storage of metadata starts"
    broker -> meta_service "Metadata from previous tasks and post time are stored"
    meta_service -> broker "Post time is returned"
    broker -> kweeter "Post time is added to saga"
    # cache
    kweeter -> broker "Caching of kweet data starts"
    broker -> kweader_cache_service "All data from previous steps is stored"
    kweader_cache_service -> broker "Caching completed"
    broker -> kweeter "Saga is finalized"
}

# container access_manager "AccessServices" {
#     include *
#     # autoLayout
# }

# container post_manager "PostingServices" {
#     include *
#     exclude relationship.tag==tag_kweader
#     exclude relationship.tag==tag_timeline
#     # autoLayout
# }

# container trend_manager "TrendServices" {
#     include *
#     # autoLayout
# }

# container interaction_manager "InteractionServices" {
#     include *
#     # autoLayout
# }

# container kweet_reader "ReaderServices" {
#     include *
#     # autoLayout
# }