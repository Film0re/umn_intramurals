module.exports = {
  root: true,
  rules: {
    "no-console": ["warn", { "allow": ["warn", "error"] }],
    "prefer-arrow-callback": "warn",
    "arrow-body-style": ["warn", "as-needed"],
    "func-style": ["warn", "expression"],
    "arrow-spacing": ["warn", { "before": true, "after": true }],
    "spaced-comment": ["warn", "always"],
    "vue/max-attributes-per-line": ["warn", {
      "singleline": 3,
      "multiline": 1,
    }],
    "sort-imports": ["warn", {
        "ignoreCase": false,
        "ignoreDeclarationSort": true,
        "ignoreMemberSort": true,
        "memberSyntaxSortOrder": ["none", "all", "multiple", "single"],
        "allowSeparatedGroups": false
    }],
},

  extends: ["@nuxt/eslint-config"],
};