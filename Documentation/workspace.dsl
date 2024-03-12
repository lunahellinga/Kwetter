workspace "Kwetter" {
    !adrs decisions
    !docs docs
    !docs docs/analysis
    !docs docs/design
    !docs docs/testing
    !docs docs/infra
    !docs docs/security
    !docs docs/dev
    !docs docs/sla

    !constant "WEB" "Angular"
    !constant "PYTHON" "Python 3.11"
    !constant "NET" ".NET Core 7.0"
    !constant "SQL" "PostgreSQL"
    !constant "DISTSQL" "Yugabyte"
    !constant "NOSQL" "MongoDB"
    !constant "CACHE" "Redis"
    !constant "TIME" "Time Series Database"
    !constant "TBD" "TBD"
    !constant "MSG_HTTPS" "HTTPS"
    !constant "MSG_JSON" "HTTPS"
    !constant "MSG_MT" "Masstransit (AMQP 0-9-1)"


    model {
        !include dsl/research_model.dsl
        group "Kwetter" {
            !include dsl/architecture_model.dsl
        }
        group "Initial Design" {
            !include dsl/planned_architecture_model.dsl
        }



    }
    views {
        !include dsl/research_views.dsl
        !include dsl/architecture_views.dsl
        !include dsl/planned_architecture_views.dsl
        !include dsl/styles.dsl
    }
}
