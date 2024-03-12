group "Research"{
    rs_ss_research = softwareSystem "Research 1: Introduction"{
        !docs ../docs/research
    }
    
    rs_ss_scalability = softwareSystem "Research 2: Scalability"{
        !docs ../docs/research/scalability
            rs_sca_webapp = container "FindThatCar Webapp" "" "" "Web Browser"
            rs_sca_vehicle_mon = container "Monolith Vehicle Search Application"
            rs_sca_webapp -> rs_sca_vehicle_mon
            rs_sca_vehicle_x_bal = container "VS Load Balancer" "Distribute requests evenly" "" "Broker"
            rs_sca_vehicle_x_1 = container "Vehicle Search App X1"
            rs_sca_vehicle_x_2 = container "Vehicle Search App X2"
            rs_sca_webapp -> rs_sca_vehicle_x_bal
            rs_sca_vehicle_x_bal -> rs_sca_vehicle_x_1
            rs_sca_vehicle_x_bal -> rs_sca_vehicle_x_2
            rs_sca_vehicle_z_router = container "Vehicle Type Router" "Route by: vehicle_type" "" "Broker"
            rs_sca_vehicle_z_1 = container "Vehicle Search App Z1" "Searches for cars"
            rs_sca_vehicle_z_2 = container "Vehicle Search App Z2" "Searches for trucks"
            rs_sca_vehicle_z_3 = container "Vehicle Search App Z3" "Searches for buses"
            
            rs_sca_webapp -> rs_sca_vehicle_z_router
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_1
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_2
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_3
            
            rs_sca_vehicle_y_gate = container "API Gateway"
            rs_sca_vehicle_y_1 = container "VS QuickSearch Service" "Uses heavy caching to perform quick lookups"
            rs_sca_vehicle_y_2 = container "VS FullSearch Service" "Performs database searches if quicksearch fails"
            rs_sca_vehicle_y_3 = container "VS Image Service" "Fetches images from the image store"
            rs_sca_vehicle_y_4 = container "VS QuickData Service" "Performs optimized database lookups based on QuickSearch results"
        
            rs_sca_webapp -> rs_sca_vehicle_y_gate
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_1
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_2
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_3
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_4
        }


    rs_ss_data = softwareSystem "Research 3: Data"{
        !docs ../docs/research/data
    }
}