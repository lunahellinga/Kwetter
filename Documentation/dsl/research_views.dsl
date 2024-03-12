dynamic rs_ss_scalability "rs_scale_mon" {
    title "Example: Monolith"
    autoLayout lr
    rs_sca_webapp -> rs_sca_vehicle_mon "Send search request"
    rs_sca_vehicle_mon -> rs_sca_webapp "Return result"
}
dynamic rs_ss_scalability "rs_scale_x" {
    title "Example: X-axis"
    rs_sca_webapp -> rs_sca_vehicle_x_bal "Send request"
    {
        {
            rs_sca_vehicle_x_bal -> rs_sca_vehicle_x_1 "Forward 1/2 of requests"
            rs_sca_vehicle_x_1 -> rs_sca_vehicle_x_bal "Return result"
            rs_sca_vehicle_x_bal -> rs_sca_webapp "Return result"
        }
        {
            rs_sca_vehicle_x_bal -> rs_sca_vehicle_x_2 "Forward 1/2 of requests"
            rs_sca_vehicle_x_2 -> rs_sca_vehicle_x_bal "Return result"
            rs_sca_vehicle_x_bal -> rs_sca_webapp "Return result"
        }
    }
}
dynamic rs_ss_scalability "rs_scale_z" {
    title "Example: Z-axis"
    rs_sca_webapp -> rs_sca_vehicle_z_router "Send request"
    {
        {
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_1 "Forward vehicle_type=car"
            rs_sca_vehicle_z_1 -> rs_sca_vehicle_z_router "Return result"
            rs_sca_vehicle_z_router -> rs_sca_webapp "Return result"
    
        }
        {
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_2 "Forward vehicle_type=truck"
            rs_sca_vehicle_z_2 -> rs_sca_vehicle_z_router "Return result"
            rs_sca_vehicle_z_router -> rs_sca_webapp "Return result"
        }
        {
            rs_sca_vehicle_z_router -> rs_sca_vehicle_z_3 "Forward vehicle_type=bus"
            rs_sca_vehicle_z_3 -> rs_sca_vehicle_z_router "Return result"
            rs_sca_vehicle_z_router -> rs_sca_webapp "Return result"
        }
    }
}
dynamic rs_ss_scalability "rs_scale_y_fs" {
    title "Example: Y-axis FS"
    rs_sca_webapp -> rs_sca_vehicle_y_gate "Send request"
    rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_1 "Send search params"
    rs_sca_vehicle_y_1 -> rs_sca_vehicle_y_gate "If found return ID, else None"
    rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_2 "If None, send search params"
    rs_sca_vehicle_y_2 -> rs_sca_vehicle_y_gate "If found return ID and requested data"
    rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_3 "If ID, send image request"
    rs_sca_vehicle_y_3 -> rs_sca_vehicle_y_gate "Return image"
    rs_sca_vehicle_y_gate -> rs_sca_webapp "Return data and image or none"
    }
dynamic rs_ss_scalability "rs_scale_y_qs" {
    title "Example: Y-axis QS"
    rs_sca_webapp -> rs_sca_vehicle_y_gate "Send request"
    rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_1 "Send search params"
    rs_sca_vehicle_y_1 -> rs_sca_vehicle_y_gate "If found return ID, else None"
    {
        {
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_4 "If ID, send ID and requested params"
            rs_sca_vehicle_y_4 -> rs_sca_vehicle_y_gate "Return requested data"
            rs_sca_vehicle_y_gate -> rs_sca_webapp "Return result"
        }
        {
            rs_sca_vehicle_y_gate -> rs_sca_vehicle_y_3 "If ID, send image request"
            rs_sca_vehicle_y_3 -> rs_sca_vehicle_y_gate "Return image"
            rs_sca_vehicle_y_gate -> rs_sca_webapp "Return result"
        }
    }
}