use strict;
# Static Lexically Scoped Global Variable
our $global = 20;

sub fn1 {
    # Lexically scoped variable taking the value
    # of $global
    my $s = $global;
    # global unchanged
    print "global in fn1 is ${s}\n";
}

sub fn2 {
    # Dynamically scoped global $x without implicit declaration
    $global = shift;
    # global changes here to parameter
    print "global in fn2 is ${global}\n";
}

sub fn3 {
    # Lexically scoped variable with the same name as global
    local $global = 200;
    # Local $global = 200
    # static $global = 20
    print "global inside fn3 is ${global} \n";
}

fn1();
fn2(500);
fn1();
fn3();