
SELECT 
     TR_Key 
    ,TR_Value 
FROM T_Translations 
WHERE TR_Key LIKE 'default_issue_status_%'

ORDER BY TR_Value 


-- TR_Key                             TR_Value
-- default_issue_status_closed        Closed
-- default_issue_status_feedback      Feedback
-- default_issue_status_in_progress   In Progress
-- default_issue_status_new           New
-- default_issue_status_rejected      Rejected
-- default_issue_status_resolved      Resolved


-- TR_Key                             TR_Value
-- default_issue_status_closed        Erledigt
-- default_issue_status_feedback      Feedback
-- default_issue_status_in_progress   In Bearbeitung
-- default_issue_status_new           Neu
-- default_issue_status_rejected      Abgewiesen
-- default_issue_status_resolved      Gelöst
