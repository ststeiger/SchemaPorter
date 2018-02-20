
using System.Collections.Generic;


namespace SchemaPorter.SimpleYamlSchema
{

    public class Formats
    {
        public string @default { get; set; }
        public string @short { get; set; }
        public string @long { get; set; }
    }

    
    public class Date
    {
        public Formats formats { get; set; }

        // [YamlDotNet.Serialization.YamlMember(Alias = "day_names")]
        public List<string> day_names { get; set; }

        public List<string> abbr_day_names { get; set; }
        public List<string> month_names { get; set; }
        public List<string> abbr_month_names { get; set; }
        public List<string> order { get; set; }
    }

    public class Formats2
    {
        public string @default { get; set; }
        public string time { get; set; }
        public string @short { get; set; }
        public string @long { get; set; }
    }

    public class Time
    {
        public Formats2 formats { get; set; }
        public string am { get; set; }
        public string pm { get; set; }
    }

    public class LessThanXSeconds
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class XSeconds
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class LessThanXMinutes
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class XMinutes
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class AboutXHours
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class XHours
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class XDays
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class AboutXMonths
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class XMonths
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class AboutXYears
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class OverXYears
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class AlmostXYears
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class DistanceInWords
    {
        public string half_a_minute { get; set; }
        public LessThanXSeconds less_than_x_seconds { get; set; }
        public XSeconds x_seconds { get; set; }
        public LessThanXMinutes less_than_x_minutes { get; set; }
        public XMinutes x_minutes { get; set; }
        public AboutXHours about_x_hours { get; set; }
        public XHours x_hours { get; set; }
        public XDays x_days { get; set; }
        public AboutXMonths about_x_months { get; set; }
        public XMonths x_months { get; set; }
        public AboutXYears about_x_years { get; set; }
        public OverXYears over_x_years { get; set; }
        public AlmostXYears almost_x_years { get; set; }
    }

    public class Datetime
    {
        public DistanceInWords distance_in_words { get; set; }
    }

    public class Format
    {
        public string separator { get; set; }
        public string delimiter { get; set; }
        public string precision { get; set; }
    }

    public class Format2
    {
        public string unit { get; set; }
        public string format { get; set; }
        public string delimiter { get; set; }
    }

    public class Currency
    {
        public Format2 format { get; set; }
    }

    public class Format3
    {
        public string delimiter { get; set; }
    }

    public class Percentage
    {
        public Format3 format { get; set; }
    }

    public class Format4
    {
        public string delimiter { get; set; }
    }

    public class Precision
    {
        public Format4 format { get; set; }
    }

    public class Format5
    {
        public string delimiter { get; set; }
        public string precision { get; set; }
    }

    public class Byte
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class Units
    {
        public Byte @byte { get; set; }
        public string kb { get; set; }
        public string mb { get; set; }
        public string gb { get; set; }
        public string tb { get; set; }
    }

    public class StorageUnits
    {
        public string format { get; set; }
        public Units units { get; set; }
    }

    public class Human
    {
        public Format5 format { get; set; }
        public StorageUnits storage_units { get; set; }
    }

    public class Number
    {
        public Format format { get; set; }
        public Currency currency { get; set; }
        public Percentage percentage { get; set; }
        public Precision precision { get; set; }
        public Human human { get; set; }
    }

    public class Array
    {
        public string sentence_connector { get; set; }
        public string skip_last_comma { get; set; }
    }

    public class Support
    {
        public Array array { get; set; }
    }

    public class Header
    {
        public string one { get; set; }
        public string other { get; set; }
    }

    public class Template
    {
        public Header header { get; set; }
        public string body { get; set; }
    }

    public class Messages
    {
        public string inclusion { get; set; }
        public string exclusion { get; set; }
        public string invalid { get; set; }
        public string confirmation { get; set; }
        public string accepted { get; set; }
        public string empty { get; set; }
        public string blank { get; set; }
        public string too_long { get; set; }
        public string too_short { get; set; }
        public string wrong_length { get; set; }
        public string taken { get; set; }
        public string not_a_number { get; set; }
        public string not_a_date { get; set; }
        public string greater_than { get; set; }
        public string greater_than_or_equal_to { get; set; }
        public string equal_to { get; set; }
        public string less_than { get; set; }
        public string less_than_or_equal_to { get; set; }
        public string odd { get; set; }
        public string even { get; set; }
        public string greater_than_start_date { get; set; }
        public string not_same_project { get; set; }
        public string circular_dependency { get; set; }
        public string cant_link_an_issue_with_a_descendant { get; set; }
        public string earlier_than_minimum_start_date { get; set; }
    }

    public class Errors
    {
        public Template template { get; set; }
        public Messages messages { get; set; }
    }

    public class Activerecord
    {
        public Errors errors { get; set; }
    }

    public class LabelXClosedIssuesAbbr
    {
        public string zero { get; set; }
        public string one { get; set; }
        public string other { get; set; }
    }

    public class LabelXComments
    {
        public string zero { get; set; }
        public string one { get; set; }
        public string other { get; set; }
    }

    public class LabelXIssues
    {
        public string zero { get; set; }
        public string one { get; set; }
        public string other { get; set; }
    }

    public class LabelXOpenIssuesAbbr
    {
        public string zero { get; set; }
        public string one { get; set; }
        public string other { get; set; }
    }

    public class LabelXProjects
    {
        public string zero { get; set; }
        public string one { get; set; }
        public string other { get; set; }
    }

    public class DE
    {
        public string direction { get; set; }
        public Date date { get; set; }
        public Time time { get; set; }
        public Datetime datetime { get; set; }
        public Number number { get; set; }
        public Support support { get; set; }
        public Activerecord activerecord { get; set; }
        public string actionview_instancetag_blank_option { get; set; }
        public string button_activate { get; set; }
        public string button_add { get; set; }
        public string button_annotate { get; set; }
        public string button_apply { get; set; }
        public string button_archive { get; set; }
        public string button_back { get; set; }
        public string button_cancel { get; set; }
        public string button_change { get; set; }
        public string button_change_password { get; set; }
        public string button_check_all { get; set; }
        public string button_clear { get; set; }
        public string button_close { get; set; }
        public string button_collapse_all { get; set; }
        public string button_configure { get; set; }
        public string button_copy { get; set; }
        public string button_copy_and_follow { get; set; }
        public string button_create { get; set; }
        public string button_create_and_continue { get; set; }
        public string button_delete { get; set; }
        public string button_delete_my_account { get; set; }
        public string button_download { get; set; }
        public string button_duplicate { get; set; }
        public string button_edit { get; set; }
        public string button_edit_associated_wikipage { get; set; }
        public string button_edit_section { get; set; }
        public string button_expand_all { get; set; }
        public string button_export { get; set; }
        public string button_hide { get; set; }
        public string button_list { get; set; }
        public string button_lock { get; set; }
        public string button_log_time { get; set; }
        public string button_login { get; set; }
        public string button_move { get; set; }
        public string button_move_and_follow { get; set; }
        public string button_quote { get; set; }
        public string button_rename { get; set; }
        public string button_reopen { get; set; }
        public string button_reply { get; set; }
        public string button_reset { get; set; }
        public string button_rollback { get; set; }
        public string button_save { get; set; }
        public string button_show { get; set; }
        public string button_sort { get; set; }
        public string button_submit { get; set; }
        public string button_test { get; set; }
        public string button_unarchive { get; set; }
        public string button_uncheck_all { get; set; }
        public string button_unlock { get; set; }
        public string button_unwatch { get; set; }
        public string button_update { get; set; }
        public string button_view { get; set; }
        public string button_watch { get; set; }
        public string default_activity_design { get; set; }
        public string default_activity_development { get; set; }
        public string default_doc_category_tech { get; set; }
        public string default_doc_category_user { get; set; }
        public string default_issue_status_closed { get; set; }
        public string default_issue_status_feedback { get; set; }
        public string default_issue_status_in_progress { get; set; }
        public string default_issue_status_new { get; set; }
        public string default_issue_status_rejected { get; set; }
        public string default_issue_status_resolved { get; set; }
        public string default_priority_high { get; set; }
        public string default_priority_immediate { get; set; }
        public string default_priority_low { get; set; }
        public string default_priority_normal { get; set; }
        public string default_priority_urgent { get; set; }
        public string default_role_developer { get; set; }
        public string default_role_manager { get; set; }
        public string default_role_reporter { get; set; }
        public string default_tracker_bug { get; set; }
        public string default_tracker_feature { get; set; }
        public string default_tracker_support { get; set; }
        public string description_all_columns { get; set; }
        public string description_available_columns { get; set; }
        public string description_choose_project { get; set; }
        public string description_date_from { get; set; }
        public string description_date_range_interval { get; set; }
        public string description_date_range_list { get; set; }
        public string description_date_to { get; set; }
        public string description_filter { get; set; }
        public string description_issue_category_reassign { get; set; }
        public string description_message_content { get; set; }
        public string description_notes { get; set; }
        public string description_project_scope { get; set; }
        public string description_query_sort_criteria_attribute { get; set; }
        public string description_query_sort_criteria_direction { get; set; }
        public string description_search { get; set; }
        public string description_selected_columns { get; set; }
        public string description_user_mail_notification { get; set; }
        public string description_wiki_subpages_reassign { get; set; }
        public string enumeration_activities { get; set; }
        public string enumeration_doc_categories { get; set; }
        public string enumeration_issue_priorities { get; set; }
        public string enumeration_system_activity { get; set; }
        public string error_attachment_too_big { get; set; }
        public string error_can_not_archive_project { get; set; }
        public string error_can_not_delete_custom_field { get; set; }
        public string error_can_not_delete_tracker { get; set; }
        public string error_can_not_remove_role { get; set; }
        public string error_can_not_reopen_issue_on_closed_version { get; set; }
        public string error_can_t_load_default_data { get; set; }
        public string error_issue_done_ratios_not_updated { get; set; }
        public string error_issue_not_found_in_project { get; set; }
        public string error_no_default_issue_status { get; set; }
        public string error_no_tracker_in_project { get; set; }
        public string error_scm_annotate { get; set; }
        public string error_scm_annotate_big_text_file { get; set; }
        public string error_scm_command_failed { get; set; }
        public string error_scm_not_found { get; set; }
        public string error_session_expired { get; set; }
        public string error_unable_delete_issue_status { get; set; }
        public string error_unable_to_connect { get; set; }
        public string error_workflow_copy_source { get; set; }
        public string error_workflow_copy_target { get; set; }
        public string field_account { get; set; }
        public string field_active { get; set; }
        public string field_activity { get; set; }
        public string field_admin { get; set; }
        public string field_assignable { get; set; }
        public string field_assigned_to { get; set; }
        public string field_assigned_to_role { get; set; }
        public string field_attr_firstname { get; set; }
        public string field_attr_lastname { get; set; }
        public string field_attr_login { get; set; }
        public string field_attr_mail { get; set; }
        public string field_auth_source { get; set; }
        public string field_auth_source_ldap_filter { get; set; }
        public string field_author { get; set; }
        public string field_base_dn { get; set; }
        public string field_board_parent { get; set; }
        public string field_category { get; set; }
        public string field_column_names { get; set; }
        public string field_closed_on { get; set; }
        public string field_comments { get; set; }
        public string field_comments_sorting { get; set; }
        public string field_commit_logs_encoding { get; set; }
        public string field_content { get; set; }
        public string field_core_fields { get; set; }
        public string field_created_on { get; set; }
        public string field_cvs_module { get; set; }
        public string field_cvsroot { get; set; }
        public string field_default_value { get; set; }
        public string field_default_status { get; set; }
        public string field_delay { get; set; }
        public string field_description { get; set; }
        public string field_done_ratio { get; set; }
        public string field_downloads { get; set; }
        public string field_due_date { get; set; }
        public string field_editable { get; set; }
        public string field_effective_date { get; set; }
        public string field_estimated_hours { get; set; }
        public string field_field_format { get; set; }
        public string field_filename { get; set; }
        public string field_filesize { get; set; }
        public string field_firstname { get; set; }
        public string field_fixed_version { get; set; }
        public string field_generate_password { get; set; }
        public string field_group_by { get; set; }
        public string field_hide_mail { get; set; }
        public string field_homepage { get; set; }
        public string field_host { get; set; }
        public string field_hours { get; set; }
        public string field_identifier { get; set; }
        public string field_identity_url { get; set; }
        public string field_inherit_members { get; set; }
        public string field_is_closed { get; set; }
        public string field_is_default { get; set; }
        public string field_is_filter { get; set; }
        public string field_is_for_all { get; set; }
        public string field_is_in_roadmap { get; set; }
        public string field_is_private { get; set; }
        public string field_is_public { get; set; }
        public string field_is_required { get; set; }
        public string field_issue { get; set; }
        public string field_issue_to { get; set; }
        public string field_issues_visibility { get; set; }
        public string field_language { get; set; }
        public string field_last_login_on { get; set; }
        public string field_lastname { get; set; }
        public string field_login { get; set; }
        public string field_mail { get; set; }
        public string field_mail_notification { get; set; }
        public string field_max_length { get; set; }
        public string field_member_of_group { get; set; }
        public string field_min_length { get; set; }
        public string field_multiple { get; set; }
        public string field_must_change_passwd { get; set; }
        public string field_name { get; set; }
        public string field_new_password { get; set; }
        public string field_notes { get; set; }
        public string field_onthefly { get; set; }
        public string field_parent { get; set; }
        public string field_parent_issue { get; set; }
        public string field_parent_title { get; set; }
        public string field_password { get; set; }
        public string field_password_confirmation { get; set; }
        public string field_path_to_repository { get; set; }
        public string field_port { get; set; }
        public string field_possible_values { get; set; }
        public string field_principal { get; set; }
        public string field_priority { get; set; }
        public string field_private_notes { get; set; }
        public string field_project { get; set; }
        public string field_redirect_existing_links { get; set; }
        public string field_regexp { get; set; }
        public string field_repository_is_default { get; set; }
        public string field_role { get; set; }
        public string field_root_directory { get; set; }
        public string field_scm_path_encoding { get; set; }
        public string field_searchable { get; set; }
        public string field_sharing { get; set; }
        public string field_spent_on { get; set; }
        public string field_start_date { get; set; }
        public string field_start_page { get; set; }
        public string field_status { get; set; }
        public string field_subject { get; set; }
        public string field_subproject { get; set; }
        public string field_summary { get; set; }
        public string field_text { get; set; }
        public string field_time_entries { get; set; }
        public string field_time_zone { get; set; }
        public string field_timeout { get; set; }
        public string field_title { get; set; }
        public string field_tracker { get; set; }
        public string field_type { get; set; }
        public string field_updated_on { get; set; }
        public string field_url { get; set; }
        public string field_user { get; set; }
        public string field_users_visibility { get; set; }
        public string field_value { get; set; }
        public string field_version { get; set; }
        public string field_visible { get; set; }
        public string field_warn_on_leaving_unsaved { get; set; }
        public string field_watcher { get; set; }
        public string general_csv_decimal_separator { get; set; }
        public string general_csv_encoding { get; set; }
        public string general_csv_separator { get; set; }
        public string general_pdf_fontname { get; set; }
        public string general_pdf_monospaced_fontname { get; set; }
        public string general_first_day_of_week { get; set; }
        public string general_lang_name { get; set; }
        public string general_text_No { get; set; }
        public string general_text_Yes { get; set; }
        public string general_text_no { get; set; }
        public string general_text_yes { get; set; }
        public string label_activity { get; set; }
        public string label_add_another_file { get; set; }
        public string label_add_note { get; set; }
        public string label_add_projects { get; set; }
        public string label_added { get; set; }
        public string label_added_time_by { get; set; }
        public string label_additional_workflow_transitions_for_assignee { get; set; }
        public string label_additional_workflow_transitions_for_author { get; set; }
        public string label_administration { get; set; }
        public string label_age { get; set; }
        public string label_ago { get; set; }
        public string label_all { get; set; }
        public string label_all_time { get; set; }
        public string label_all_words { get; set; }
        public string label_and_its_subprojects { get; set; }
        public string label_any { get; set; }
        public string label_any_issues_in_project { get; set; }
        public string label_any_issues_not_in_project { get; set; }
        public string label_api_access_key { get; set; }
        public string label_api_access_key_created_on { get; set; }
        public string label_applied_status { get; set; }
        public string label_ascending { get; set; }
        public string label_ask { get; set; }
        public string label_assigned_to_me_issues { get; set; }
        public string label_associated_revisions { get; set; }
        public string label_attachment { get; set; }
        public string label_attachment_delete { get; set; }
        public string label_attachment_new { get; set; }
        public string label_attachment_plural { get; set; }
        public string label_attribute { get; set; }
        public string label_attribute_of_assigned_to { get; set; }
        public string label_attribute_of_author { get; set; }
        public string label_attribute_of_fixed_version { get; set; }
        public string label_attribute_of_issue { get; set; }
        public string label_attribute_of_project { get; set; }
        public string label_attribute_of_user { get; set; }
        public string label_attribute_plural { get; set; }
        public string label_auth_source { get; set; }
        public string label_auth_source_new { get; set; }
        public string label_auth_source_plural { get; set; }
        public string label_authentication { get; set; }
        public string label_between { get; set; }
        public string label_blocked_by { get; set; }
        public string label_blocks { get; set; }
        public string label_board { get; set; }
        public string label_board_locked { get; set; }
        public string label_board_new { get; set; }
        public string label_board_plural { get; set; }
        public string label_board_sticky { get; set; }
        public string label_boolean { get; set; }
        public string label_branch { get; set; }
        public string label_browse { get; set; }
        public string label_bulk_edit_selected_issues { get; set; }
        public string label_bulk_edit_selected_time_entries { get; set; }
        public string label_calendar { get; set; }
        public string label_change_plural { get; set; }
        public string label_change_properties { get; set; }
        public string label_change_status { get; set; }
        public string label_change_view_all { get; set; }
        public string label_changes_details { get; set; }
        public string label_changeset_plural { get; set; }
        public string label_checkboxes { get; set; }
        public string label_check_for_updates { get; set; }
        public string label_child_revision { get; set; }
        public string label_chronological_order { get; set; }
        public string label_close_versions { get; set; }
        public string label_closed_issues { get; set; }
        public string label_closed_issues_plural { get; set; }
        public string label_comment { get; set; }
        public string label_comment_add { get; set; }
        public string label_comment_added { get; set; }
        public string label_comment_delete { get; set; }
        public string label_comment_plural { get; set; }
        public string label_commits_per_author { get; set; }
        public string label_commits_per_month { get; set; }
        public string label_completed_versions { get; set; }
        public string label_confirmation { get; set; }
        public string label_contains { get; set; }
        public string label_copied { get; set; }
        public string label_copied_from { get; set; }
        public string label_copied_to { get; set; }
        public string label_copy_attachments { get; set; }
        public string label_copy_same_as_target { get; set; }
        public string label_copy_source { get; set; }
        public string label_copy_subtasks { get; set; }
        public string label_copy_target { get; set; }
        public string label_copy_workflow_from { get; set; }
        public string label_cross_project_descendants { get; set; }
        public string label_cross_project_hierarchy { get; set; }
        public string label_cross_project_system { get; set; }
        public string label_cross_project_tree { get; set; }
        public string label_current_status { get; set; }
        public string label_current_version { get; set; }
        public string label_custom_field { get; set; }
        public string label_custom_field_new { get; set; }
        public string label_custom_field_plural { get; set; }
        public string label_custom_field_select_type { get; set; }
        public string label_date { get; set; }
        public string label_date_from { get; set; }
        public string label_date_from_to { get; set; }
        public string label_date_range { get; set; }
        public string label_date_to { get; set; }
        public string label_day_plural { get; set; }
        public string label_default { get; set; }
        public string label_default_columns { get; set; }
        public string label_deleted { get; set; }
        public string label_descending { get; set; }
        public string label_details { get; set; }
        public string label_diff { get; set; }
        public string label_diff_inline { get; set; }
        public string label_diff_side_by_side { get; set; }
        public string label_disabled { get; set; }
        public string label_display { get; set; }
        public string label_display_per_page { get; set; }
        public string label_display_used_statuses_only { get; set; }
        public string label_document { get; set; }
        public string label_document_added { get; set; }
        public string label_document_new { get; set; }
        public string label_document_plural { get; set; }
        public string label_downloads_abbr { get; set; }
        public string label_drop_down_list { get; set; }
        public string label_duplicated_by { get; set; }
        public string label_duplicates { get; set; }
        public string label_edit_attachments { get; set; }
        public string label_enumeration_new { get; set; }
        public string label_enumerations { get; set; }
        public string label_environment { get; set; }
        public string label_equals { get; set; }
        public string label_example { get; set; }
        public string label_export_options { get; set; }
        public string label_export_to { get; set; }
        public string label_f_hour { get; set; }
        public string label_f_hour_plural { get; set; }
        public string label_feed_plural { get; set; }
        public string label_feeds_access_key { get; set; }
        public string label_feeds_access_key_created_on { get; set; }
        public string label_fields_permissions { get; set; }
        public string label_file_added { get; set; }
        public string label_file_plural { get; set; }
        public string label_filter_add { get; set; }
        public string label_filter_plural { get; set; }
        public string label_float { get; set; }
        public string label_follows { get; set; }
        public string label_gantt { get; set; }
        public string label_gantt_progress_line { get; set; }
        public string label_general { get; set; }
        public string label_generate_key { get; set; }
        public string label_git_report_last_commit { get; set; }
        public string label_greater_or_equal { get; set; }
        public string label_group { get; set; }
        public string label_group_anonymous { get; set; }
        public string label_group_new { get; set; }
        public string label_group_non_member { get; set; }
        public string label_group_plural { get; set; }
        public string label_help { get; set; }
        public string label_hidden { get; set; }
        public string label_history { get; set; }
        public string label_home { get; set; }
        public string label_in { get; set; }
        public string label_in_less_than { get; set; }
        public string label_in_more_than { get; set; }
        public string label_in_the_next_days { get; set; }
        public string label_in_the_past_days { get; set; }
        public string label_incoming_emails { get; set; }
        public string label_index_by_date { get; set; }
        public string label_index_by_title { get; set; }
        public string label_information { get; set; }
        public string label_information_plural { get; set; }
        public string label_integer { get; set; }
        public string label_internal { get; set; }
        public string label_issue { get; set; }
        public string label_issue_added { get; set; }
        public string label_issue_assigned_to_updated { get; set; }
        public string label_issue_category { get; set; }
        public string label_issue_category_new { get; set; }
        public string label_issue_category_plural { get; set; }
        public string label_issue_new { get; set; }
        public string label_issue_note_added { get; set; }
        public string label_issue_plural { get; set; }
        public string label_issue_priority_updated { get; set; }
        public string label_issue_status { get; set; }
        public string label_issue_status_new { get; set; }
        public string label_issue_status_plural { get; set; }
        public string label_issue_status_updated { get; set; }
        public string label_issue_tracking { get; set; }
        public string label_issue_updated { get; set; }
        public string label_issue_view_all { get; set; }
        public string label_issue_watchers { get; set; }
        public string label_issues_by { get; set; }
        public string label_issues_visibility_all { get; set; }
        public string label_issues_visibility_own { get; set; }
        public string label_issues_visibility_public { get; set; }
        public string label_item_position { get; set; }
        public string label_jump_to_a_project { get; set; }
        public string label_language_based { get; set; }
        public string label_last_changes { get; set; }
        public string label_last_login { get; set; }
        public string label_last_month { get; set; }
        public string label_last_n_days { get; set; }
        public string label_last_n_weeks { get; set; }
        public string label_last_week { get; set; }
        public string label_latest_compatible_version { get; set; }
        public string label_latest_revision { get; set; }
        public string label_latest_revision_plural { get; set; }
        public string label_ldap_authentication { get; set; }
        public string label_less_or_equal { get; set; }
        public string label_less_than_ago { get; set; }
        public string label_link { get; set; }
        public string label_link_copied_issue { get; set; }
        public string label_link_values_to { get; set; }
        public string label_list { get; set; }
        public string label_loading { get; set; }
        public string label_logged_as { get; set; }
        public string label_login { get; set; }
        public string label_login_with_open_id_option { get; set; }
        public string label_logout { get; set; }
        public string label_only { get; set; }
        public string label_max_size { get; set; }
        public string label_me { get; set; }
        public string label_member { get; set; }
        public string label_member_new { get; set; }
        public string label_member_plural { get; set; }
        public string label_message_last { get; set; }
        public string label_message_new { get; set; }
        public string label_message_plural { get; set; }
        public string label_message_posted { get; set; }
        public string label_min_max_length { get; set; }
        public string label_missing_api_access_key { get; set; }
        public string label_missing_feeds_access_key { get; set; }
        public string label_modified { get; set; }
        public string label_module_plural { get; set; }
        public string label_month { get; set; }
        public string label_months_from { get; set; }
        public string label_more { get; set; }
        public string label_more_than_ago { get; set; }
        public string label_my_account { get; set; }
        public string label_my_page { get; set; }
        public string label_my_page_block { get; set; }
        public string label_my_projects { get; set; }
        public string label_my_queries { get; set; }
        public string label_new { get; set; }
        public string label_new_statuses_allowed { get; set; }
        public string label_news { get; set; }
        public string label_news_added { get; set; }
        public string label_news_comment_added { get; set; }
        public string label_news_latest { get; set; }
        public string label_news_new { get; set; }
        public string label_news_plural { get; set; }
        public string label_news_view_all { get; set; }
        public string label_next { get; set; }
        public string label_no_change_option { get; set; }
        public string label_no_data { get; set; }
        public string label_no_issues_in_project { get; set; }
        public string label_nobody { get; set; }
        public string label_none { get; set; }
        public string label_not_contains { get; set; }
        public string label_not_equals { get; set; }
        public string label_open_issues { get; set; }
        public string label_open_issues_plural { get; set; }
        public string label_optional_description { get; set; }
        public string label_options { get; set; }
        public string label_overall_activity { get; set; }
        public string label_overall_spent_time { get; set; }
        public string label_overview { get; set; }
        public string label_parent_revision { get; set; }
        public string label_password_lost { get; set; }
        public string label_password_required { get; set; }
        public string label_permissions { get; set; }
        public string label_permissions_report { get; set; }
        public string label_personalize_page { get; set; }
        public string label_planning { get; set; }
        public string label_please_login { get; set; }
        public string label_plugins { get; set; }
        public string label_precedes { get; set; }
        public string label_preferences { get; set; }
        public string label_preview { get; set; }
        public string label_previous { get; set; }
        public string label_principal_search { get; set; }
        public string label_profile { get; set; }
        public string label_project { get; set; }
        public string label_project_all { get; set; }
        public string label_project_copy_notifications { get; set; }
        public string label_project_latest { get; set; }
        public string label_project_new { get; set; }
        public string label_project_plural { get; set; }
        public string label_public_projects { get; set; }
        public string label_query { get; set; }
        public string label_query_new { get; set; }
        public string label_query_plural { get; set; }
        public string label_radio_buttons { get; set; }
        public string label_read { get; set; }
        public string label_readonly { get; set; }
        public string label_register { get; set; }
        public string label_registered_on { get; set; }
        public string label_registration_activation_by_email { get; set; }
        public string label_registration_automatic_activation { get; set; }
        public string label_registration_manual_activation { get; set; }
        public string label_related_issues { get; set; }
        public string label_relates_to { get; set; }
        public string label_relation_delete { get; set; }
        public string label_relation_new { get; set; }
        public string label_renamed { get; set; }
        public string label_reply_plural { get; set; }
        public string label_report { get; set; }
        public string label_report_plural { get; set; }
        public string label_reported_issues { get; set; }
        public string label_repository { get; set; }
        public string label_repository_new { get; set; }
        public string label_repository_plural { get; set; }
        public string label_required { get; set; }
        public string label_result_plural { get; set; }
        public string label_reverse_chronological_order { get; set; }
        public string label_revision { get; set; }
        public string label_revision_id { get; set; }
        public string label_revision_plural { get; set; }
        public string label_roadmap { get; set; }
        public string label_roadmap_due_in { get; set; }
        public string label_roadmap_no_issues { get; set; }
        public string label_roadmap_overdue { get; set; }
        public string label_role { get; set; }
        public string label_role_and_permissions { get; set; }
        public string label_role_anonymous { get; set; }
        public string label_role_new { get; set; }
        public string label_role_non_member { get; set; }
        public string label_role_plural { get; set; }
        public string label_scm { get; set; }
        public string label_search { get; set; }
        public string label_search_for_watchers { get; set; }
        public string label_search_titles_only { get; set; }
        public string label_send_information { get; set; }
        public string label_send_test_email { get; set; }
        public string label_session_expiration { get; set; }
        public string label_settings { get; set; }
        public string label_show_closed_projects { get; set; }
        public string label_show_completed_versions { get; set; }
        public string label_sort { get; set; }
        public string label_sort_by { get; set; }
        public string label_sort_higher { get; set; }
        public string label_sort_highest { get; set; }
        public string label_sort_lower { get; set; }
        public string label_sort_lowest { get; set; }
        public string label_spent_time { get; set; }
        public string label_statistics { get; set; }
        public string label_status_transitions { get; set; }
        public string label_stay_logged_in { get; set; }
        public string label_string { get; set; }
        public string label_subproject_new { get; set; }
        public string label_subproject_plural { get; set; }
        public string label_subtask_plural { get; set; }
        public string label_tag { get; set; }
        public string label_text { get; set; }
        public string label_theme { get; set; }
        public string label_this_month { get; set; }
        public string label_this_week { get; set; }
        public string label_this_year { get; set; }
        public string label_time_entry_plural { get; set; }
        public string label_time_tracking { get; set; }
        public string label_today { get; set; }
        public string label_topic_plural { get; set; }
        public string label_total { get; set; }
        public string label_total_time { get; set; }
        public string label_tracker { get; set; }
        public string label_tracker_new { get; set; }
        public string label_tracker_plural { get; set; }
        public string label_unknown_plugin { get; set; }
        public string label_update_issue_done_ratios { get; set; }
        public string label_updated_time { get; set; }
        public string label_updated_time_by { get; set; }
        public string label_used_by { get; set; }
        public string label_user { get; set; }
        public string label_user_activity { get; set; }
        public string label_user_anonymous { get; set; }
        public string label_user_mail_no_self_notified { get; set; }
        public string label_user_mail_option_all { get; set; }
        public string label_user_mail_option_none { get; set; }
        public string label_user_mail_option_only_assigned { get; set; }
        public string label_user_mail_option_only_my_events { get; set; }
        public string label_user_mail_option_only_owner { get; set; }
        public string label_user_mail_option_selected { get; set; }
        public string label_user_new { get; set; }
        public string label_user_plural { get; set; }
        public string label_user_search { get; set; }
        public string label_users_visibility_all { get; set; }
        public string label_users_visibility_members_of_visible_projects { get; set; }
        public string label_version { get; set; }
        public string label_version_new { get; set; }
        public string label_version_plural { get; set; }
        public string label_version_sharing_descendants { get; set; }
        public string label_version_sharing_hierarchy { get; set; }
        public string label_version_sharing_none { get; set; }
        public string label_version_sharing_system { get; set; }
        public string label_version_sharing_tree { get; set; }
        public string label_view_all_revisions { get; set; }
        public string label_view_diff { get; set; }
        public string label_view_revisions { get; set; }
        public string label_visibility_private { get; set; }
        public string label_visibility_public { get; set; }
        public string label_visibility_roles { get; set; }
        public string label_watched_issues { get; set; }
        public string label_week { get; set; }
        public string label_wiki { get; set; }
        public string label_wiki_content_added { get; set; }
        public string label_wiki_content_updated { get; set; }
        public string label_wiki_edit { get; set; }
        public string label_wiki_edit_plural { get; set; }
        public string label_wiki_page { get; set; }
        public string label_wiki_page_plural { get; set; }
        public string label_workflow { get; set; }
        public LabelXClosedIssuesAbbr label_x_closed_issues_abbr { get; set; }
        public LabelXComments label_x_comments { get; set; }
        public LabelXIssues label_x_issues { get; set; }
        public LabelXOpenIssuesAbbr label_x_open_issues_abbr { get; set; }
        public LabelXProjects label_x_projects { get; set; }
        public string label_year { get; set; }
        public string label_yesterday { get; set; }
        public string mail_body_account_activation_request { get; set; }
        public string mail_body_account_information { get; set; }
        public string mail_body_account_information_external { get; set; }
        public string mail_body_lost_password { get; set; }
        public string mail_body_register { get; set; }
        public string mail_body_reminder { get; set; }
        public string mail_body_wiki_content_added { get; set; }
        public string mail_body_wiki_content_updated { get; set; }
        public string mail_subject_account_activation_request { get; set; }
        public string mail_subject_lost_password { get; set; }
        public string mail_subject_register { get; set; }
        public string mail_subject_reminder { get; set; }
        public string mail_subject_wiki_content_added { get; set; }
        public string mail_subject_wiki_content_updated { get; set; }
        public string notice_account_activated { get; set; }
        public string notice_account_deleted { get; set; }
        public string notice_account_invalid_creditentials { get; set; }
        public string notice_account_lost_email_sent { get; set; }
        public string notice_account_locked { get; set; }
        public string notice_account_not_activated_yet { get; set; }
        public string notice_account_password_updated { get; set; }
        public string notice_account_pending { get; set; }
        public string notice_account_register_done { get; set; }
        public string notice_account_unknown_email { get; set; }
        public string notice_account_updated { get; set; }
        public string notice_account_wrong_password { get; set; }
        public string notice_api_access_key_reseted { get; set; }
        public string notice_can_t_change_password { get; set; }
        public string notice_default_data_loaded { get; set; }
        public string notice_email_error { get; set; }
        public string notice_email_sent { get; set; }
        public string notice_failed_to_save_issues { get; set; }
        public string notice_failed_to_save_members { get; set; }
        public string notice_failed_to_save_time_entries { get; set; }
        public string notice_feeds_access_key_reseted { get; set; }
        public string notice_file_not_found { get; set; }
        public string notice_gantt_chart_truncated { get; set; }
        public string notice_issue_done_ratios_updated { get; set; }
        public string notice_issue_successful_create { get; set; }
        public string notice_issue_update_conflict { get; set; }
        public string notice_locking_conflict { get; set; }
        public string notice_new_password_must_be_different { get; set; }
        public string notice_no_issue_selected { get; set; }
        public string notice_not_authorized { get; set; }
        public string notice_not_authorized_archived_project { get; set; }
        public string notice_successful_connection { get; set; }
        public string notice_successful_create { get; set; }
        public string notice_successful_delete { get; set; }
        public string notice_successful_update { get; set; }
        public string notice_unable_delete_time_entry { get; set; }
        public string notice_unable_delete_version { get; set; }
        public string notice_user_successful_create { get; set; }
        public string permission_add_issue_notes { get; set; }
        public string permission_add_issue_watchers { get; set; }
        public string permission_add_issues { get; set; }
        public string permission_add_messages { get; set; }
        public string permission_add_project { get; set; }
        public string permission_add_subprojects { get; set; }
        public string permission_add_documents { get; set; }
        public string permission_browse_repository { get; set; }
        public string permission_close_project { get; set; }
        public string permission_comment_news { get; set; }
        public string permission_commit_access { get; set; }
        public string permission_delete_issue_watchers { get; set; }
        public string permission_delete_issues { get; set; }
        public string permission_delete_messages { get; set; }
        public string permission_delete_own_messages { get; set; }
        public string permission_delete_wiki_pages { get; set; }
        public string permission_delete_wiki_pages_attachments { get; set; }
        public string permission_delete_documents { get; set; }
        public string permission_edit_issue_notes { get; set; }
        public string permission_edit_issues { get; set; }
        public string permission_edit_messages { get; set; }
        public string permission_edit_own_issue_notes { get; set; }
        public string permission_edit_own_messages { get; set; }
        public string permission_edit_own_time_entries { get; set; }
        public string permission_edit_project { get; set; }
        public string permission_edit_time_entries { get; set; }
        public string permission_edit_wiki_pages { get; set; }
        public string permission_edit_documents { get; set; }
        public string permission_export_wiki_pages { get; set; }
        public string permission_log_time { get; set; }
        public string permission_manage_boards { get; set; }
        public string permission_manage_categories { get; set; }
        public string permission_manage_files { get; set; }
        public string permission_manage_issue_relations { get; set; }
        public string permission_manage_members { get; set; }
        public string permission_manage_news { get; set; }
        public string permission_manage_project_activities { get; set; }
        public string permission_manage_public_queries { get; set; }
        public string permission_manage_related_issues { get; set; }
        public string permission_manage_repository { get; set; }
        public string permission_manage_subtasks { get; set; }
        public string permission_manage_versions { get; set; }
        public string permission_manage_wiki { get; set; }
        public string permission_move_issues { get; set; }
        public string permission_protect_wiki_pages { get; set; }
        public string permission_rename_wiki_pages { get; set; }
        public string permission_save_queries { get; set; }
        public string permission_select_project_modules { get; set; }
        public string permission_set_issues_private { get; set; }
        public string permission_set_notes_private { get; set; }
        public string permission_set_own_issues_private { get; set; }
        public string permission_view_calendar { get; set; }
        public string permission_view_changesets { get; set; }
        public string permission_view_documents { get; set; }
        public string permission_view_files { get; set; }
        public string permission_view_gantt { get; set; }
        public string permission_view_issue_watchers { get; set; }
        public string permission_view_issues { get; set; }
        public string permission_view_messages { get; set; }
        public string permission_view_private_notes { get; set; }
        public string permission_view_time_entries { get; set; }
        public string permission_view_wiki_edits { get; set; }
        public string permission_view_wiki_pages { get; set; }
        public string project_module_boards { get; set; }
        public string project_module_calendar { get; set; }
        public string project_module_documents { get; set; }
        public string project_module_files { get; set; }
        public string project_module_gantt { get; set; }
        public string project_module_issue_tracking { get; set; }
        public string project_module_news { get; set; }
        public string project_module_repository { get; set; }
        public string project_module_time_tracking { get; set; }
        public string project_module_wiki { get; set; }
        public string project_status_active { get; set; }
        public string project_status_archived { get; set; }
        public string project_status_closed { get; set; }
        public string setting_activity_days_default { get; set; }
        public string setting_app_subtitle { get; set; }
        public string setting_app_title { get; set; }
        public string setting_attachment_max_size { get; set; }
        public string setting_autofetch_changesets { get; set; }
        public string setting_autologin { get; set; }
        public string setting_bcc_recipients { get; set; }
        public string setting_cache_formatted_text { get; set; }
        public string setting_commit_cross_project_ref { get; set; }
        public string setting_commit_fix_keywords { get; set; }
        public string setting_commit_logtime_activity_id { get; set; }
        public string setting_commit_logtime_enabled { get; set; }
        public string setting_commit_ref_keywords { get; set; }
        public string setting_cross_project_issue_relations { get; set; }
        public string setting_cross_project_subtasks { get; set; }
        public string setting_date_format { get; set; }
        public string setting_default_issue_start_date_to_creation_date { get; set; }
        public string setting_default_language { get; set; }
        public string setting_default_notification_option { get; set; }
        public string setting_default_projects_modules { get; set; }
        public string setting_default_projects_public { get; set; }
        public string setting_default_projects_tracker_ids { get; set; }
        public string setting_diff_max_lines_displayed { get; set; }
        public string setting_display_subprojects_issues { get; set; }
        public string setting_emails_footer { get; set; }
        public string setting_emails_header { get; set; }
        public string setting_enabled_scm { get; set; }
        public string setting_feeds_limit { get; set; }
        public string setting_file_max_size_displayed { get; set; }
        public string setting_force_default_language_for_anonymous { get; set; }
        public string setting_force_default_language_for_loggedin { get; set; }
        public string setting_gantt_items_limit { get; set; }
        public string setting_gravatar_default { get; set; }
        public string setting_gravatar_enabled { get; set; }
        public string setting_host_name { get; set; }
        public string setting_issue_done_ratio { get; set; }
        public string setting_issue_done_ratio_issue_field { get; set; }
        public string setting_issue_done_ratio_issue_status { get; set; }
        public string setting_issue_group_assignment { get; set; }
        public string setting_issue_list_default_columns { get; set; }
        public string setting_issues_export_limit { get; set; }
        public string setting_jsonp_enabled { get; set; }
        public string setting_link_copied_issue { get; set; }
        public string setting_login_required { get; set; }
        public string setting_mail_from { get; set; }
        public string setting_mail_handler_api_enabled { get; set; }
        public string setting_mail_handler_api_key { get; set; }
        public string setting_mail_handler_body_delimiters { get; set; }
        public string setting_mail_handler_excluded_filenames { get; set; }
        public string setting_new_project_user_role_id { get; set; }
        public string setting_non_working_week_days { get; set; }
        public string setting_openid { get; set; }
        public string setting_password_min_length { get; set; }
        public string setting_password_max_age { get; set; }
        public string setting_per_page_options { get; set; }
        public string setting_plain_text_mail { get; set; }
        public string setting_protocol { get; set; }
        public string setting_repositories_encodings { get; set; }
        public string setting_repository_log_display_limit { get; set; }
        public string setting_rest_api_enabled { get; set; }
        public string setting_self_registration { get; set; }
        public string setting_sequential_project_identifiers { get; set; }
        public string setting_session_lifetime { get; set; }
        public string setting_session_timeout { get; set; }
        public string setting_start_of_week { get; set; }
        public string setting_sys_api_enabled { get; set; }
        public string setting_text_formatting { get; set; }
        public string setting_thumbnails_enabled { get; set; }
        public string setting_thumbnails_size { get; set; }
        public string setting_time_format { get; set; }
        public string setting_unsubscribe { get; set; }
        public string setting_user_format { get; set; }
        public string setting_welcome_text { get; set; }
        public string setting_wiki_compression { get; set; }
        public string status_active { get; set; }
        public string status_locked { get; set; }
        public string status_registered { get; set; }
        public string text_account_destroy_confirmation { get; set; }
        public string text_are_you_sure { get; set; }
        public string text_assign_time_entries_to_project { get; set; }
        public string text_caracters_maximum { get; set; }
        public string text_caracters_minimum { get; set; }
        public string text_comma_separated { get; set; }
        public string text_convert_available { get; set; }
        public string text_custom_field_possible_values_info { get; set; }
        public string text_default_administrator_account_changed { get; set; }
        public string text_destroy_time_entries { get; set; }
        public string text_destroy_time_entries_question { get; set; }
        public string text_diff_truncated { get; set; }
        public string text_email_delivery_not_configured { get; set; }
        public string text_enumeration_category_reassign_to { get; set; }
        public string text_enumeration_destroy_question { get; set; }
        public string text_file_repository_writable { get; set; }
        public string text_git_repository_note { get; set; }
        public string text_issue_added { get; set; }
        public string text_issue_category_destroy_assignments { get; set; }
        public string text_issue_category_destroy_question { get; set; }
        public string text_issue_category_reassign_to { get; set; }
        public string text_issue_conflict_resolution_add_notes { get; set; }
        public string text_issue_conflict_resolution_cancel { get; set; }
        public string text_issue_conflict_resolution_overwrite { get; set; }
        public string text_issue_updated { get; set; }
        public string text_issues_destroy_confirmation { get; set; }
        public string text_issues_destroy_descendants_confirmation { get; set; }
        public string text_issues_ref_in_commit_messages { get; set; }
        public string text_journal_added { get; set; }
        public string text_journal_changed { get; set; }
        public string text_journal_changed_no_detail { get; set; }
        public string text_journal_deleted { get; set; }
        public string text_journal_set_to { get; set; }
        public string text_length_between { get; set; }
        public string text_line_separated { get; set; }
        public string text_load_default_configuration { get; set; }
        public string text_mercurial_repository_note { get; set; }
        public string text_min_max_length_info { get; set; }
        public string text_no_configuration_data { get; set; }
        public string text_own_membership_delete_confirmation { get; set; }
        public string text_plugin_assets_writable { get; set; }
        public string text_project_closed { get; set; }
        public string text_project_destroy_confirmation { get; set; }
        public string text_project_identifier_info { get; set; }
        public string text_reassign_time_entries { get; set; }
        public string text_regexp_info { get; set; }
        public string text_repository_identifier_info { get; set; }
        public string text_repository_usernames_mapping { get; set; }
        public string text_rmagick_available { get; set; }
        public string text_scm_command { get; set; }
        public string text_scm_command_not_available { get; set; }
        public string text_scm_command_version { get; set; }
        public string text_scm_config { get; set; }
        public string text_scm_path_encoding_note { get; set; }
        public string text_select_mail_notifications { get; set; }
        public string text_select_project_modules { get; set; }
        public string text_session_expiration_settings { get; set; }
        public string text_status_changed_by_changeset { get; set; }
        public string text_subprojects_destroy_warning { get; set; }
        public string text_subversion_repository_note { get; set; }
        public string text_time_entries_destroy_confirmation { get; set; }
        public string text_time_logged_by_changeset { get; set; }
        public string text_tip_issue_begin_day { get; set; }
        public string text_tip_issue_begin_end_day { get; set; }
        public string text_tip_issue_end_day { get; set; }
        public string text_tracker_no_workflow { get; set; }
        public string text_turning_multiple_off { get; set; }
        public string text_unallowed_characters { get; set; }
        public string text_user_mail_option { get; set; }
        public string text_user_wrote { get; set; }
        public string text_warn_on_leaving_unsaved { get; set; }
        public string text_wiki_destroy_confirmation { get; set; }
        public string text_wiki_page_destroy_children { get; set; }
        public string text_wiki_page_destroy_question { get; set; }
        public string text_wiki_page_nullify_children { get; set; }
        public string text_wiki_page_reassign_children { get; set; }
        public string text_workflow_edit { get; set; }
        public string text_zoom_in { get; set; }
        public string text_zoom_out { get; set; }
        public string version_status_closed { get; set; }
        public string version_status_locked { get; set; }
        public string version_status_open { get; set; }
        public string warning_attachments_not_saved { get; set; }
        public string label_search_attachments_yes { get; set; }
        public string label_search_attachments_no { get; set; }
        public string label_search_attachments_only { get; set; }
        public string label_search_open_issues_only { get; set; }
        public string field_address { get; set; }
        public string setting_max_additional_emails { get; set; }
        public string label_email_address_plural { get; set; }
        public string label_email_address_add { get; set; }
        public string label_enable_notifications { get; set; }
        public string label_disable_notifications { get; set; }
        public string setting_search_results_per_page { get; set; }
        public string label_blank_value { get; set; }
        public string permission_copy_issues { get; set; }
        public string error_password_expired { get; set; }
        public string field_time_entries_visibility { get; set; }
        public string label_parent_task_attributes { get; set; }
        public string label_parent_task_attributes_derived { get; set; }
        public string label_parent_task_attributes_independent { get; set; }
        public string label_time_entries_visibility_all { get; set; }
        public string label_time_entries_visibility_own { get; set; }
        public string label_member_management { get; set; }
        public string label_member_management_all_roles { get; set; }
        public string label_member_management_selected_roles_only { get; set; }
        public string label_total_spent_time { get; set; }
        public string notice_import_finished { get; set; }
        public string notice_import_finished_with_errors { get; set; }
        public string error_invalid_file_encoding { get; set; }
        public string error_invalid_csv_file_or_settings { get; set; }
        public string error_can_not_read_import_file { get; set; }
        public string permission_import_issues { get; set; }
        public string label_import_issues { get; set; }
        public string label_select_file_to_import { get; set; }
        public string label_fields_separator { get; set; }
        public string label_fields_wrapper { get; set; }
        public string label_encoding { get; set; }
        public string label_comma_char { get; set; }
        public string label_semi_colon_char { get; set; }
        public string label_quote_char { get; set; }
        public string label_double_quote_char { get; set; }
        public string label_fields_mapping { get; set; }
        public string label_file_content_preview { get; set; }
        public string label_create_missing_values { get; set; }
        public string button_import { get; set; }
        public string field_total_estimated_hours { get; set; }
        public string label_api { get; set; }
        public string label_total_plural { get; set; }
        public string label_assigned_issues { get; set; }
        public string label_field_format_enumeration { get; set; }
        public string label_f_hour_short { get; set; }
        public string field_default_version { get; set; }
        public string error_attachment_extension_not_allowed { get; set; }
        public string setting_attachment_extensions_allowed { get; set; }
        public string setting_attachment_extensions_denied { get; set; }
        public string label_any_open_issues { get; set; }
        public string label_no_open_issues { get; set; }
        public string label_default_values_for_new_users { get; set; }
    }

    public class RootObject
    {
        public DE de { get; set; }
        public DE en { get; set; }
    }


}
