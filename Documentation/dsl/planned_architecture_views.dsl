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

container old_access_manager "AccessServices" {
    include *
    # autoLayout
}

container old_post_manager "PostingServices" {
    include *
    exclude relationship.tag==tag_kweader
    exclude relationship.tag==tag_timeline
    # autoLayout
}

container old_trend_manager "TrendServices" {
    include *
    # autoLayout
}

container old_interaction_manager "InteractionServices" {
    include *
    # autoLayout
}

container old_kweet_reader "ReaderServices" {
    include *
    # autoLayout
}